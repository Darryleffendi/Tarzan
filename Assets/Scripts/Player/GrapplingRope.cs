using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapplingRope : MonoBehaviour
{
    private LineRenderer lr;
    private PlayerGrappling grappling;
    private Spring spring;
    private Vector3 currentGrapplePosition;
    public float damper;
    public float strength;
    public float velocity;
    public float waveCount;
    public float waveHeight;
    public AnimationCurve effectCurve;
    public int quality;

    private void Start()
    {
        grappling = GetComponent<PlayerGrappling>();
        lr = grappling.gunTip.GetComponent<LineRenderer>();
        spring = new Spring();
        spring.SetTarget(0);
    }

    private void LateUpdate()
    {
        DrawRope();
    }


    void DrawRope()
    {
        if (!grappling.isGrappling)
        {
            currentGrapplePosition = grappling.gunTip.position;
            spring.Reset();
            if (lr.positionCount > 0)
                lr.positionCount = 0;
            return;
        }

        if (lr.positionCount == 0)
        {
            spring.SetVelocity(velocity);
            lr.positionCount = quality + 1;
        }

        spring.SetDamper(damper);
        spring.SetStrength(strength);
        spring.Update(Time.deltaTime);

        Vector3 grapplePoint = grappling.grapplePoint;
        Vector3 gunTip = grappling.gunTip.position;
        Vector3 up = Quaternion.LookRotation((grapplePoint - gunTip).normalized) * Vector3.up;

        currentGrapplePosition = Vector3.Lerp(currentGrapplePosition, grapplePoint, Time.deltaTime * 8f);

        for (int i = 0; i < quality + 1; i++)
        {
            float delta = i / (float)quality;
            Vector3 offset = up * waveHeight * Mathf.Sin(delta * waveCount * Mathf.PI * spring.Value * effectCurve.Evaluate(delta));
            lr.SetPosition(i, Vector3.Lerp(gunTip, currentGrapplePosition, delta) + offset);
        }
    }
}
