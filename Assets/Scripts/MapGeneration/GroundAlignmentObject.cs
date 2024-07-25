using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundAlignmentObject : MonoBehaviour
{
    public void Align()
    {
        Vector3 curPos = transform.position;
        curPos.y = Terrain.activeTerrain.SampleHeight(curPos);
        transform.position = curPos;
    }
}
