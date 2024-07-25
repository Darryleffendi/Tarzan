using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WFCNode
{
    public NodeQuadrant facingTo;
    WFCObject wfcObj;

    public List<int> possibleObjectIndex;

    public WFCNode()
    {
        possibleObjectIndex = new List<int>
        {
            0,1,2,3,4,5,6,7,8,9
        };
    }

    public void UpdatePossibility(WFCObject obj)
    {
        foreach(int idx in obj.neighborRestrictions)
        {
                possibleObjectIndex.Remove(idx);
        }
    }
}
