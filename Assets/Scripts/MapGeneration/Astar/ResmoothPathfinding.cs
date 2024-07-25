using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResmoothPathfinding : MonoBehaviour
{
    private Grid grid;
    private WaveFunctionCollapse wfc;
    private List<Node> initialPath;
    private List<Node> finalPath;

    private void Awake()
    {
        grid = Grid.Instance;
    }

    public Vector3[] GeneratePath(List<Node> pointNodes, List<Node> initPath)
    {
        wfc = WaveFunctionCollapse.Instance;
        initialPath = initPath;
        finalPath = new List<Node>();

        Node startNode = pointNodes[0];
        // Find starting node (Center edge node)
        foreach (Node node in pointNodes)
        {
            if (node.isCenterPoint)
            {
                startNode = node;
                break;
            }
        }

        FindPath(startNode, grid.GetCenterNode());
        return SimplifyPath(finalPath);
    }

    void FindPath(Node startNode, Node targetNode)
    {
        bool pathSuccess = false;

        // Set containing nodes to be evaluated
        Heap frontierSet = new Heap(grid.gridAmountX * grid.gridAmountY + 1);
        if (startNode != null) frontierSet.Insert(startNode);
        // Set containing nodes that has been evaluated
        HashSet<Node> exploredSet = new HashSet<Node>();

        while (frontierSet.Size > 0)
        {
            // Get smallest value from heap
            Node curr = frontierSet.GetMin();
            exploredSet.Add(curr);

            // If node is found, proceed to backtracing
            if (curr == targetNode)
            {
                pathSuccess = true;
                break;
            }

            // Iterate over neighbors
            foreach (Node neighbor in grid.GetNeighbors(curr))
            {
                // If not walkable or if exists in closed set, do not proceed
                if (!initialPath.Contains(neighbor) || exploredSet.Contains(neighbor))
                {
                    if (neighbor != targetNode)
                        continue;
                }

                int costToNeighbor = curr.gCost + Node.Heuristic(curr, neighbor);
                bool inFrontier = frontierSet.Contains(neighbor);

                if (!inFrontier || costToNeighbor < neighbor.gCost)
                {
                    neighbor.gCost = costToNeighbor;
                    neighbor.hCost = Node.Heuristic(neighbor, targetNode);
                    neighbor.parent = curr;

                    if (!inFrontier) frontierSet.Insert(neighbor);
                }
            }

        }
        if (pathSuccess)
        {
            BackTrace(startNode, targetNode);
        }
    }

    void BackTrace(Node startNode, Node targetNode)
    {
        Node curr = targetNode;

        while (curr != startNode)
        {
            finalPath.Add(curr);
            curr.isPath = true;
            curr.parent.wfcNode.facingTo = grid.GetNodeDirection(curr.parent, curr);

            int objIndex = 10;
            if (curr.wfcNode.facingTo != curr.parent.wfcNode.facingTo)
                objIndex = 11;

            wfc.Collapse(curr, objIndex);

            curr = curr.parent;
        }

        finalPath.Reverse();
    }

    Vector3[] SimplifyPath(List<Node> path)
    {
        List<Vector3> waypoints = new List<Vector3>();
        Vector2 oldDirection = Vector2.zero;
        int lastIndexPoint = 0;

        for (int i = 1; i < path.Count; i++)
        {
            Vector2 newDirection = new Vector2(path[i - 1].xGrid - path[i].xGrid, path[i - 1].yGrid - path[i].yGrid);
            float yDiff = Mathf.Abs(path[lastIndexPoint].worldPos.y - path[i].worldPos.y);

            if (newDirection != oldDirection || yDiff > 0.5f)
            {
                lastIndexPoint = i;
                waypoints.Add(path[i].worldPos);
            }
            oldDirection = newDirection;
        }
        return waypoints.ToArray();
    }
}
