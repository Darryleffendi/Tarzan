using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    // 2D array of nodes
    Node[,] grid;

    [Header("Grid Sizing")]
    [SerializeField]
    protected Vector2 worldSize;
    [SerializeField]
    protected float nodeRadius;
    protected float nodeDiameter;
    [SerializeField]
    protected LayerMask unpassableLayer;
    [Header("Gizmos Settings")]
    [SerializeField]
    protected bool drawGrid;
    [SerializeField]
    protected bool drawInitialPath, drawQuadrants, drawQuadrantBorders, drawNodeDirection = false;

    [HideInInspector]
    public int gridAmountX, gridAmountY, gridAmountMin;
    [HideInInspector]
    public List<Node> northPoints, eastPoints, southPoints, westPoints;
    private int gridHeight = 10;

    public static Grid Instance { get; private set; }

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

        nodeDiameter = nodeRadius * 2;
        gridAmountX = Mathf.RoundToInt(worldSize.x / nodeDiameter);
        gridAmountY = Mathf.RoundToInt(worldSize.y / nodeDiameter);
        gridAmountMin = Mathf.Min(gridAmountX, gridAmountY);

        northPoints = new List<Node>();
        eastPoints = new List<Node>();
        southPoints = new List<Node>();
        westPoints = new List<Node>();

        InitializeGrid();
    }

    void InitializeGrid()
    {
        // Initialize 2D array of size [Xsize, Ysize]
        grid = new Node[gridAmountX, gridAmountY];
        Vector3 bottomLeft = transform.position - (Vector3.right * worldSize.x / 2) - (Vector3.forward * worldSize.y / 2);

        for (int i = 0; i < gridAmountY; i++)
        {
            for (int j = 0; j < gridAmountX; j++)
            {
                Vector3 worldPoint = bottomLeft + Vector3.right * (j * nodeDiameter + nodeRadius) + Vector3.forward * (i * nodeDiameter + nodeRadius);
                worldPoint.y = GetTerrainHeight(worldPoint);

                bool isWalkable = !Physics.CheckSphere(worldPoint, nodeRadius, unpassableLayer);

                Node newNode = new Node(isWalkable, worldPoint, j, i);
                
                for(int h = -1; h <= 1; h ++)
                    if (i == j + h|| i + j == gridAmountMin + h)
                    {
                        newNode.isQuadrantBorder = true;
                        newNode.isWalkable = false;
                    }

                isCenterEdgeNode(newNode, true);
                grid[j, i] = newNode;
            }
        }

        // Make center node walkable
        for(int i = -2; i <= 2; i ++)
        {
            for(int j = -2; j <= 2; j ++)
            {
                grid[gridAmountX / 2 + i, gridAmountY / 2 + j].isQuadrantBorder = false;
                grid[gridAmountX / 2 + i, gridAmountY / 2 + j].isWalkable = true;
            }
        }

        for(int i = 0; i < 4; i ++)
        {
            RandomQuadrantNode(NodeQuadrant.North, true, true);
            RandomQuadrantNode(NodeQuadrant.East, true, true);
            RandomQuadrantNode(NodeQuadrant.South, true, true);
            RandomQuadrantNode(NodeQuadrant.West, true, true);
        }
    }

    public NodeQuadrant CheckQuadrant(int x, int y)
    {
        if(x > y)
        {
            if(x + y > gridAmountMin)
            {
                return NodeQuadrant.East;
            }
            else
            {
                return NodeQuadrant.South;
            }
        }
        else
        {
            if (x + y > gridAmountMin)
            {
                return NodeQuadrant.North;
            }
            else
            {
                return NodeQuadrant.West;
            }
        }
    }
    private bool isCenterEdgeNode(Node node, bool MarkAsPoint = false)
    {
        while (true)
        {
            if(node.xGrid == gridAmountX / 2)
            {
                if (node.quadrant == NodeQuadrant.North && node.yGrid == gridAmountY - 1)
                    break;
                else if (node.quadrant == NodeQuadrant.South && node.yGrid == 0)
                    break;
            }
            else if(node.yGrid == gridAmountY / 2)
            {
                if (node.quadrant == NodeQuadrant.East && node.xGrid == gridAmountX - 1)
                    break;
                else if (node.quadrant == NodeQuadrant.West && node.xGrid == 0)
                    break;
            }
            return false;
        }

        if(MarkAsPoint)
        {
            node.isPoint = true;
            node.isCenterPoint = true;
            if (node.quadrant == NodeQuadrant.North) northPoints.Add(node);
            if (node.quadrant == NodeQuadrant.East) eastPoints.Add(node);
            if (node.quadrant == NodeQuadrant.South) southPoints.Add(node);
            if (node.quadrant == NodeQuadrant.West) westPoints.Add(node);
        }

        return true;
    }

    public NodeQuadrant GetNodeDirection(Node currNode, Node nextNode)
    {
        if (nextNode.xGrid < currNode.xGrid)
            return NodeQuadrant.West;
        else if (nextNode.xGrid > currNode.xGrid)
            return NodeQuadrant.East;
        else if (nextNode.yGrid > currNode.yGrid)
            return NodeQuadrant.North;
        else
            return NodeQuadrant.South;
    }

    public Node GetCenterNode()
    {
        return grid[gridAmountX / 2, gridAmountY / 2];
    }

    private Node RandomQuadrantNode(NodeQuadrant quadrant, bool insertToList = false, bool markNode = false)
    {
        int xBotRange, xTopRange, yBotRange, yTopRange;
        List<Node> nodeList;
        Node node;

        while (true)
        {
            if (quadrant == NodeQuadrant.North)
            {
                xBotRange = 0;
                xTopRange = gridAmountX;
                yBotRange = gridAmountY / 2;
                yTopRange = gridAmountY;
                nodeList = northPoints;
            }
            else if (quadrant == NodeQuadrant.East)
            {
                xBotRange = gridAmountX / 2;
                xTopRange = gridAmountX;
                yBotRange = 0;
                yTopRange = gridAmountY;
                nodeList = eastPoints;
            }
            else if (quadrant == NodeQuadrant.South)
            {
                xBotRange = 0;
                xTopRange = gridAmountX;
                yBotRange = 0;
                yTopRange = gridAmountY / 2;
                nodeList = southPoints;
            }
            else
            {
                xBotRange = 0;
                xTopRange = gridAmountX / 2;
                yBotRange = 0;
                yTopRange = gridAmountY;
                nodeList = westPoints;
            }

            int xGrid = Random.Range(xBotRange, xTopRange);
            int yGrid = Random.Range(yBotRange, yTopRange);

            node = grid[xGrid, yGrid];
            if (node.quadrant == quadrant 
                && !nodeList.Contains(node) 
                && node.isWalkable
                && !isCenterEdgeNode(node))
                break;
        }

        if (markNode)
        {
            grid[node.xGrid, node.yGrid].isPoint = true;
            node.isPoint = true;
        }
        if (insertToList) nodeList.Add(node);
        return node;
    }

    public Node NodeFromPos(Vector3 pos)
    {
        float xPercent = Mathf.Clamp01((pos.x - transform.position.x + worldSize.x / 2) / worldSize.x);
        float yPercent = Mathf.Clamp01((pos.z - transform.position.z + worldSize.y / 2) / worldSize.y);

        int x = Mathf.FloorToInt(gridAmountX * xPercent);
        int y = Mathf.FloorToInt(gridAmountY * yPercent);

        return grid[x, y];
    }

    public List<Node> GetNeighbors(Node node)
    {
        List<Node> neighbors = new List<Node>();

        for (int i = -1; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                // If currentNode or diagonalNode, dont add to list
                if (Mathf.Abs(i) == Mathf.Abs(j)) continue;

                int coorX = node.xGrid + j;
                int coorY = node.yGrid + i;

                // Make sure that node is inside of grid
                if (coorX >= 0 && coorX < gridAmountX && coorY >= 0 && coorY < gridAmountY)
                {
                    neighbors.Add(grid[coorX, coorY]);
                }
            }
        }
        return neighbors;
    }

    public float GetTerrainHeight(Vector3 worldPos)
    {
        float maxY = 0;
        foreach (Terrain terrain in Terrain.activeTerrains)
        {
            float curY = terrain.SampleHeight(worldPos);

            if (curY > maxY)
                maxY = curY;
        }
        return maxY;
    }

    // Gizmos function for testing purposes
    void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(worldSize.x, gridHeight, worldSize.y));

        if (grid == null || !drawGrid) return;

        foreach (Node node in grid)
        {
            if ((!node.isWalkable || node.isQuadrantBorder)&& drawQuadrantBorders) Gizmos.color = Color.black;
            else if (node.isPoint) Gizmos.color = Color.magenta;
            else if (node.isPath)
            {
                if (node.wfcNode.facingTo == NodeQuadrant.North && drawNodeDirection) Gizmos.color = Color.red;
                else if (node.wfcNode.facingTo == NodeQuadrant.East && drawNodeDirection) Gizmos.color = Color.yellow;
                else if (node.wfcNode.facingTo == NodeQuadrant.South && drawNodeDirection) Gizmos.color = Color.green;
                else if (node.wfcNode.facingTo == NodeQuadrant.West && drawNodeDirection) Gizmos.color = Color.blue;
                else Gizmos.color = Color.cyan;
            }
            else if (node.isInitialPath && drawInitialPath) Gizmos.color = Color.white;
            else if (node.quadrant == NodeQuadrant.North && drawQuadrants) Gizmos.color = Color.red;
            else if (node.quadrant == NodeQuadrant.East && drawQuadrants) Gizmos.color = Color.yellow;
            else if (node.quadrant == NodeQuadrant.South && drawQuadrants) Gizmos.color = Color.green;
            else if (node.quadrant == NodeQuadrant.West && drawQuadrants) Gizmos.color = Color.blue;
            else Gizmos.color = Color.clear;

            Gizmos.DrawCube(node.worldPos, Vector3.one * (nodeDiameter - .1f));
        }
    }

    public Node GetNode(int x, int y)
    {
        return grid[x, y];
    }
}
