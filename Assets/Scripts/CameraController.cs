using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    [HideInInspector]
    public static CameraController Instance { get; private set; }

    public AnimationCurve animateCurve;
    public CinemachineVirtualCamera mainCamera;
    public CinemachineVirtualCamera crouchCamera;
    private CinemachineBasicMultiChannelPerlin noise;
    private float initialAmplitude = 0.75f;
    private float newAmplitude;
    private float initialFov;
    private float newFov;
    private bool isShaking;

    void Start()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }

        initialFov = mainCamera.m_Lens.FieldOfView;
        newFov = initialFov;
        noise = mainCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        RestoreNoise();
    }

    private void Update()
    {
        if(Mathf.Abs(newFov - mainCamera.m_Lens.FieldOfView) >= 0.05)
        {
            mainCamera.m_Lens.FieldOfView = Mathf.Lerp(mainCamera.m_Lens.FieldOfView, newFov, animateCurve.Evaluate(0.16f));
        }

        if (Mathf.Abs(newAmplitude - noise.m_AmplitudeGain) >= 0.05)
        {
            noise.m_AmplitudeGain = Mathf.Lerp(noise.m_AmplitudeGain, newAmplitude, animateCurve.Evaluate(0.16f));
            noise.m_FrequencyGain = Mathf.Lerp(noise.m_FrequencyGain, newAmplitude * 2, animateCurve.Evaluate(0.16f));
        }
    }

    public void Crouch(bool x)
    {
        if (x)
        {
            crouchCamera.m_Priority = 10;
            mainCamera.m_Priority = 0;
            return;
        }
        mainCamera.m_Priority = 10;
        crouchCamera.m_Priority = 0;
    }

    public void ChangeNoise(float amplitudeGain)
    {
        if (isShaking) return;
        newAmplitude = amplitudeGain;
    }

    public void RestoreNoise()
    {
        if (isShaking) return;
        newAmplitude = initialAmplitude;
    }

    private void StopShaking()
    {
        isShaking = false;
    }

    public void CameraShake(float duration, float amount = 6f)
    {
        isShaking = true;
        newAmplitude = amount;
        Invoke(nameof(StopShaking), duration - 0.1f);
        Invoke(nameof(RestoreNoise), duration);
    }

    public bool IsDefaultNoise()
    {
        if (Mathf.Abs(newAmplitude - noise.m_AmplitudeGain) >= 0.1) return true;

        return false;
    }

    public void ChangeFov(float newFov)
    {
        this.newFov = newFov;
    }

    public void RestoreFov()
    {
        newFov = initialFov;
    }
}
