using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    // Boolean
    public bool isWalkable;
    public bool isQuadrantBorder;
    public bool isInitialPath;
    public bool isPath;
    public bool isPoint;
    public bool isCenterPoint;

    // Position
    public Vector3 worldPos;
    public int xGrid;
    public int yGrid;

    // A* Attributes
    public int gCost;
    public int hCost;
    public Node parent;

    // Misc
    public NodeQuadrant quadrant;
    public WFCNode wfcNode;

    public Node(bool walkable, Vector3 worldPos, int x, int y)
    {
        isWalkable = walkable;
        this.worldPos = worldPos;
        xGrid = x;
        yGrid = y;
        quadrant = Grid.Instance.CheckQuadrant(x, y);
        wfcNode = new WFCNode();
    }

    public int fCost
    {
        get
        {
            return gCost + hCost;
        }
    }

    // Function to calculate distance approximation h(n)
    public static int Heuristic(Node a, Node b)
    {
        int xDist = Mathf.Abs(a.xGrid - b.xGrid);
        int yDist = Mathf.Abs(a.yGrid - b.yGrid);

        if (xDist > yDist)
        {
            return 14 * yDist + 10 * (xDist - yDist);
        }
        return 14 * xDist + 10 * (yDist - xDist);
    }
}

public enum NodeQuadrant
{
    North = 0,
    East = 1,
    South = 2,
    West = 3
}
