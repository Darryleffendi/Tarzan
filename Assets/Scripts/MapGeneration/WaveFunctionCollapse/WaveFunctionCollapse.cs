using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveFunctionCollapse : MonoBehaviour
{
    [SerializeField]
    private GameObject[] objects;
    
    public static WaveFunctionCollapse Instance { get; private set; }

    // Start is called before the first frame update
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }

        for (int i = 0; i < objects.Length; i ++)
        {
            if(objects[i].GetComponent<WFCObject>() == null)
            {
                throw new System.Exception("Exception on WFC Object number " + i + " : Include WFCObject script!");
            }
        }
    }

    public void StartWave(Node startPoint)
    {
        Grid grid = Grid.Instance;

        // Breadth First Search Transversal

        Queue<Node> frontierSet = new Queue<Node>();
        HashSet<Node> exploredSet = new HashSet<Node>();

        frontierSet.Enqueue(startPoint);
        while (frontierSet.Count > 0)
        {
            Node curr = frontierSet.Dequeue();
            exploredSet.Add(curr);

            if (!curr.isPath) Collapse(curr);

            // Iterate over neighbors
            foreach (Node neighbor in grid.GetNeighbors(curr))
            {
                // If exists in closed set, do not proceed
                if (exploredSet.Contains(neighbor))
                {
                    continue;
                }
                
                if (!frontierSet.Contains(neighbor)) frontierSet.Enqueue(neighbor);
            }

        }
    }


    public GameObject[] GetPathObjects()
    {
        return new GameObject[] {
            objects[10],
            objects[11]
        };
    }

    public void Collapse(Node node)
    {
        if (!node.isWalkable && !node.isQuadrantBorder) return;

        List<int> objIdx = node.wfcNode.possibleObjectIndex;

        if (objIdx.Count <= 0) return;

        int idx = objIdx[Random.Range(0, objIdx.Count)];

        Collapse(node, idx);
    }

    public void Collapse(Node node, int index)
    {
        GameObject obj = Instantiate(objects[index], node.worldPos, transform.rotation, transform);
        Vector3 currentEulerAngles = obj.transform.eulerAngles;
        currentEulerAngles.y += GetDirectionRotation(node.wfcNode);
        obj.transform.eulerAngles = currentEulerAngles;
        obj.GetComponent<GroundAlignmentController>().Align();

        foreach (Node n in Grid.Instance.GetNeighbors(node))
        {
            n.wfcNode.UpdatePossibility(obj.GetComponent<WFCObject>());
        }
    }

    public int GetDirectionRotation(WFCNode node)
    {
        if (node.facingTo == NodeQuadrant.North)
            return 90;
        else if (node.facingTo == NodeQuadrant.East)
            return 180;
        else if (node.facingTo == NodeQuadrant.South)
            return 270;
        else
            return 0;
    }
}
