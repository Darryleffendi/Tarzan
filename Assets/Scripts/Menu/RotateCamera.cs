using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateCamera : MonoBehaviour
{
    public Transform target; 
    void Update()
    {
        transform.RotateAround(target.position, Vector3.up, 10 * Time.deltaTime);
    }
}
