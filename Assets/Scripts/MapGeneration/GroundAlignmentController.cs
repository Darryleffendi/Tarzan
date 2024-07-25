using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundAlignmentController : MonoBehaviour
{
    public void Align()
    {
        foreach (Transform child in transform)
        {
            GroundAlignmentObject obj = child.GetComponent<GroundAlignmentObject>();
            if (obj != null)
                obj.Align();
        }
    }
}
