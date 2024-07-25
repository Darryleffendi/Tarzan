using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGrappling : MonoBehaviour
{
    private PlayerMovement pm;
    private Transform cam;
    private Animator animator;
    private float grapplingCooldown = 0f;
    private float groundDrag = 0f;
    [HideInInspector]
    public bool isGrappling;
    [HideInInspector]
    public Vector3 grapplePoint;

    public float maxGrappleDistance;
    public float delay;
    public float cooldown;
    public float overshootYAxis;
    public Transform gunTip;

    void Start()
    {
        pm = GetComponent<PlayerMovement>();
        animator = GetComponent<Animator>();
        cam = Camera.main.transform;
        groundDrag = pm.GetRb().drag;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
            StartGrapple();

        if (grapplingCooldown > 0)
            grapplingCooldown -= Time.deltaTime;
    }

    private void StartGrapple()
    {
        if (grapplingCooldown > 0) return;

        isGrappling = true;
        RaycastHit hit;
        animator.SetBool("isGrappling", true);
        pm.FreezePosition(true);
        
        if(Physics.Raycast(cam.position, cam.forward, out hit, maxGrappleDistance, LayerMask.NameToLayer("Ground")) ||
            (Physics.Raycast(cam.position, cam.forward, out hit, maxGrappleDistance) && hit.transform.CompareTag("Grappleable")))
        {
            grapplePoint = hit.point;

            Invoke(nameof(ExecuteGrapple), delay);
        }
        else
        {
            grapplePoint = cam.position + cam.forward * maxGrappleDistance;

            Invoke(nameof(StopGrapple), delay);
        }
    }

    private void ExecuteGrapple()
    {
        GetComponent<PlayerAudio>().Fly();
        CameraController.Instance.CameraShake(0.3f);
        CameraController.Instance.ChangeFov(75f);
        pm.FreezePosition(false);
        pm.FreezeControls(true);
        Vector3 lowestPoint = new Vector3(transform.position.x, transform.position.y - 1f, transform.position.z);

        float grapplePointRelativeYPos = grapplePoint.y - lowestPoint.y;
        float highestPointOnArc = grapplePointRelativeYPos + overshootYAxis;

        if (grapplePointRelativeYPos < 0) highestPointOnArc = overshootYAxis;

        JumpToPosition(grapplePoint, highestPointOnArc);

        Invoke(nameof(StopGrapple), 1.5f);
    }

    private void StopGrapple()
    {
        CameraController.Instance.RestoreFov();
        isGrappling = false;
        animator.SetBool("isGrappling", false);

        grapplingCooldown = cooldown;

        pm.FreezePosition(false);
        pm.FreezeControls(false);
    }

    public void JumpToPosition(Vector3 targetPosition, float trajectoryHeight)
    {
        Vector3 swingForce = CalculateJumpForce(transform.position, targetPosition, trajectoryHeight);
        pm.GetRb().AddForce(swingForce);
        
        pm.GetRb().drag = 0;
        Invoke(nameof(ResetRestrictions), 3f);
    }

    public void ResetRestrictions()
    {
        pm.GetRb().drag = groundDrag;
    }

    public Vector3 CalculateJumpForce(Vector3 startPoint, Vector3 endPoint, float trajectoryHeight)
    { 
        float gravity = Physics.gravity.y;
        float deltaTime = Time.fixedDeltaTime;
        float displacementY = endPoint.y - startPoint.y;
        Vector3 displacementXZ = new Vector3(endPoint.x - startPoint.x, 0f, endPoint.z - startPoint.z);

        Vector3 velocityY = Vector3.up * Mathf.Sqrt(-2 * gravity * trajectoryHeight);
        Vector3 velocityXZ = displacementXZ / (Mathf.Sqrt(-2 * trajectoryHeight / gravity)
            + Mathf.Sqrt(2 * (displacementY - trajectoryHeight) / gravity));

        Vector3 desiredVelocity = velocityXZ + velocityY;
        desiredVelocity.x *= 1.75f;
        desiredVelocity.z *= 1.75f;

        Vector3 force = (desiredVelocity) / deltaTime;

        return force;
    }
}
