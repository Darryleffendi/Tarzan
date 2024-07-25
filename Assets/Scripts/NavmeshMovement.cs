using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class NavmeshMovement : MonoBehaviour
{
    private NavMeshAgent agent;
    [SerializeField]
    private AnimationCurve animationCurve;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
    }

    public void MoveToPoint(Vector3 destination)
    {
        agent.isStopped = false;
        agent.SetDestination(destination);
    }

    public void Stop(bool x)
    {
        agent.isStopped = x;
    }
    // Update is called once per frame
    void Update()
    {
        SurfaceAlignment();
        if(agent.remainingDistance < 0.35f)
        {
            agent.isStopped = true;
        }
    }

    private void SurfaceAlignment()
    {
        Ray raycast = new Ray(transform.position, -transform.up);
        RaycastHit rayInfo;

        if (Physics.Raycast(raycast, out rayInfo, 1 << LayerMask.GetMask("Ground")))
        {
            Vector3 direction = new Vector3(agent.velocity.x, 0f, agent.velocity.z);

            if (direction.magnitude < 0.01f)
            {
                direction = transform.forward;
            }
            else direction.Normalize();

            Vector3 surfaceRight = Vector3.Cross(rayInfo.normal, direction).normalized;
            Vector3 surfaceDirection = Vector3.Cross(surfaceRight, rayInfo.normal);
            Quaternion targetRotation = Quaternion.LookRotation(surfaceDirection, rayInfo.normal);

            // Smoothly rotate character based on targetRotation
            float rotationFactor = animationCurve.Evaluate(0.1f) * 40f;
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationFactor * Time.deltaTime);
        }
    }

    public void SetSpeed(float speed)
    {
        agent.speed = speed;
    }

    public float RemainingDistance()
    {
        return agent.remainingDistance;
    }
}
