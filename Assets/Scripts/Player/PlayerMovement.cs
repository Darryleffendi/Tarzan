using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private float baseSpeed;
    private float jumpSpeed = 5f;
    private float speed;
    private float coefficient = 30f;

    private bool freezePos = false;
    private bool freezeCtrl = false;
    private bool isRunning = false;
    private bool isGrounded = true;
    private bool isSwimming = false;
    private bool isCrouching = false;
    private bool isClimbing = false;

    private Rigidbody rb;
    private Animator animator;
    private PlayerAudio audio;

    private float x;
    private float y;

    // Fps Attributes
    [SerializeField]
    public CinemachineVirtualCamera camera;
    public CinemachineVirtualCamera crouchCamera;
    private float fpsXrotate = 0;
    private float fpsYrotate = 0;
    [SerializeField]
    private float rotationSmoothness = 0.2f;
    [HideInInspector]
    public Quaternion playerCurrentRotation, cameraCurrentRotation;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        audio = GetComponent<PlayerAudio>();
        speed = baseSpeed;
        playerCurrentRotation = transform.rotation;
        cameraCurrentRotation = camera.transform.rotation;
    }

    private void Update()
    {
        if (freezeCtrl)
        {
            if(freezePos)
                rb.velocity =  new Vector3(0, 1, 0) * rb.velocity.y;
            return;
        }

        animate();
        CheckJump();
        checkCrouch();
    }

    private void FixedUpdate()
    {
        movePlayer();
        FpsMovement();
    }

    public void movePlayer()
    {
        if (freezeCtrl || freezePos) return;

        x = Input.GetAxisRaw("Horizontal");
        y = Input.GetAxisRaw("Vertical");

        isRunning = Input.GetKey(KeyCode.LeftShift);
        if(isRunning && !isCrouching)
        {
            if(x != 0 || y != 0)
            {
                CameraController.Instance.ChangeFov(65f);
                CameraController.Instance.ChangeNoise(1.25f);
            }
            speed = baseSpeed * 1.6f;
        }
        else if (isCrouching)
        {
            speed = baseSpeed * 0.8f;
        }
        else
        {
            if (x != 0 || y != 0)
                CameraController.Instance.ChangeNoise(1f);
            else if(!CameraController.Instance.IsDefaultNoise())
                CameraController.Instance.RestoreNoise();

            CameraController.Instance.RestoreFov();
            speed = baseSpeed;
        }

        Vector3 forward = Camera.main.transform.forward;
        if (!isClimbing)
            forward.y = 0;
        else
            forward.y = 20;
        forward.Normalize();

        Vector3 right = Camera.main.transform.right;
        right.y = 0;
        right.Normalize();

        Vector3 relativeX = x * right;
        Vector3 relativeY = y * forward;

        // Move player
        Vector3 direction = relativeX + relativeY;
        Vector3 counterDirection = new Vector3(-rb.velocity.x, 0f, -rb.velocity.z);

        direction.y = 0;

        rb.AddForce(direction * speed);
        rb.AddForce(counterDirection * coefficient);
    }

    public void CheckJump()
    {
        // Jump
        if (Input.GetButtonDown("Jump") && (isGrounded || isClimbing))
        {
            audio.Jump();
            isGrounded = false;
            animator.SetBool("isJumping", true);
            rb.AddForce(Vector3.up * jumpSpeed, ForceMode.Impulse);
        }
    }

    public void animate()
    {
        float xVal = x;
        float yVal = y;


        if (isRunning)
        {
            xVal *= 2;
            yVal *= 2;
        }

        animator.SetFloat("Horizontal", Mathf.Lerp(animator.GetFloat("Horizontal"), xVal, rotationSmoothness));
        animator.SetFloat("Vertical", Mathf.Lerp(animator.GetFloat("Vertical"), yVal, rotationSmoothness));
    }

    public void checkCrouch()
    {
        if(Input.GetKeyDown(KeyCode.LeftControl))
        {
            isCrouching = true;
            animator.SetBool("isCrouching", true);
            CameraController.Instance.Crouch(true);
        }
        else if(Input.GetKeyUp(KeyCode.LeftControl))
        {
            isCrouching = false;
            animator.SetBool("isCrouching", false);
            CameraController.Instance.Crouch(false);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 0) isGrounded = true;
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            if(!isGrounded) audio.Land();
            isGrounded = true;
            animator.SetBool("isJumping", false);
        }
        else if (collision.gameObject.layer == LayerMask.NameToLayer("Rocks"))
        {
            if (!isGrounded) audio.Land();
            isGrounded = true;
            animator.SetBool("isJumping", false);
        }
        else if (collision.gameObject.layer == LayerMask.NameToLayer("Wood"))
        {
            if (!isGrounded) audio.Land();
            isGrounded = true;
            animator.SetBool("isJumping", false);
        }
        else if (collision.gameObject.layer == LayerMask.NameToLayer("DeepWater"))
        {
            isGrounded = true;
            isSwimming = true;
            animator.SetBool("isJumping", false);
            animator.SetBool("isSwimming", true);
        }
        else if (collision.gameObject.layer == LayerMask.NameToLayer("Trees"))
        {
            isClimbing = true;
            animator.SetBool("isClimbing", true);
        }
        audio.ChangeFootstep(collision.gameObject.layer);
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("DeepWater"))
        {
            isSwimming = false;
            animator.SetBool("isSwimming", false);
        }
        if (collision.gameObject.layer == LayerMask.NameToLayer("Trees"))
        {
            isClimbing = false;
            animator.SetBool("isClimbing", false);
        }
    }

    public bool getGroundedStatus()
    {
        return isGrounded;
    }

    private void FpsMovement()
    {
        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * 100;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * 100;

        fpsYrotate += mouseX;
        fpsXrotate = Mathf.Clamp(fpsXrotate - mouseY, -90f, 90f);

        Quaternion playerTargetRotation = Quaternion.Euler(0, fpsYrotate, 0);
        Quaternion cameraTargetRotation = Quaternion.Euler(fpsXrotate, fpsYrotate, 0);

        playerCurrentRotation = Quaternion.Lerp(playerCurrentRotation, playerTargetRotation, rotationSmoothness);
        cameraCurrentRotation = Quaternion.Lerp(cameraCurrentRotation, cameraTargetRotation, rotationSmoothness);

        transform.rotation = playerCurrentRotation;
        camera.transform.rotation = cameraCurrentRotation;
        crouchCamera.transform.rotation = cameraCurrentRotation;
    }

    public void FreezePosition(bool x)
    {
        freezePos = x;
    }
    public void FreezeControls(bool x)
    {
        freezeCtrl = x;
    }

    public Rigidbody GetRb()
    {
        return rb;
    }
}
