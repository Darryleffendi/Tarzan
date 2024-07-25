using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PauseCamera : MonoBehaviour
{
    private float fpsXrotate = 0;
    private float fpsYrotate = 0;
    private float rotationSmoothness = 0.2f;
    private PlayerMovement pm;

    private void Start()
    {
        pm = Player.Instance.GetMovement();
    }

    void Update()
    {
        if(Pause.Instance.GetPausedStatus())
        {
            float mouseX = Input.GetAxisRaw("Mouse X") * Time.unscaledDeltaTime * 100;
            float mouseY = Input.GetAxisRaw("Mouse Y") * Time.unscaledDeltaTime * 100;

            fpsYrotate += mouseX;
            fpsXrotate = Mathf.Clamp(fpsXrotate - mouseY, -90f, 90f);

            Quaternion playerTargetRotation = Quaternion.Euler(0, fpsYrotate, 0);
            Quaternion cameraTargetRotation = Quaternion.Euler(fpsXrotate, fpsYrotate, 0);

            pm.playerCurrentRotation = Quaternion.Lerp(pm.playerCurrentRotation, playerTargetRotation, rotationSmoothness);
            pm.cameraCurrentRotation = Quaternion.Lerp(pm.cameraCurrentRotation, cameraTargetRotation, rotationSmoothness);

            transform.rotation = pm.playerCurrentRotation;
            pm.camera.transform.rotation = pm.cameraCurrentRotation;
            Camera.main.GetComponent<CinemachineBrain>().enabled = false;
            Camera.main.transform.rotation = pm.cameraCurrentRotation;
        }
        else
        {
            Camera.main.GetComponent<CinemachineBrain>().enabled = true;
        }
    }
}
