using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitialPathfinding : MonoBehaviour
{
    private Grid grid;
    private List<Node> points;
    private List<Node> finalPath;

    private void Start()
    {
        grid = Grid.Instance;
    }

    public List<Node> GeneratePath(List<Node> pointNodes)
    {
        points = new List<Node>(pointNodes);
        finalPath = new List<Node>();
        Node startNode = points[0];

        // Find starting node (Center edge node)
        foreach(Node node in points)
        {
            if (node.isCenterPoint)
            {
                startNode = node;
                break;
            }
        }

        while(points.Count > 0)
        {
            points.Remove(startNode);
            Node nextNode = GetNearestTarget(startNode);

            if(nextNode != null)
            {
                FindPath(startNode, nextNode);
                startNode = nextNode;
            }
            else
            {
                FindPath(startNode, grid.GetCenterNode());
            }
        }

        finalPath.Reverse();
        return finalPath;
    }

    private Node GetNearestTarget(Node currNode)
    {
        int minDist = int.MaxValue;
        Node minNode = null;

        foreach (Node node in points)
        {
            int dist = Node.Heuristic(currNode, node);
            if (dist < minDist)
            {
                minDist = dist;
                minNode = node;
            }
        }
        return minNode;
    }

    private void FindPath(Node startNode, Node targetNode)
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
                if (!neighbor.isWalkable || exploredSet.Contains(neighbor))
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
            curr.isInitialPath = true;
            curr = curr.parent;
        }
    }
}
