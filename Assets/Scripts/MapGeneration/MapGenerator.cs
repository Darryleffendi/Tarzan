using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    private Grid grid;
    private WaveFunctionCollapse wfc;
    private InitialPathfinding initPathfinding;
    private ResmoothPathfinding resmoothPathfinding;

    private Vector3[] northPath;
    private Vector3[] eastPath;
    private Vector3[] southPath;
    private Vector3[] westPath;

    public static MapGenerator Instance { get; private set; }

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
    }

    void Start()
    {
        grid = Grid.Instance;
        wfc = WaveFunctionCollapse.Instance;
        initPathfinding = GetComponent<InitialPathfinding>();
        resmoothPathfinding = GetComponent<ResmoothPathfinding>();

        List<Node> northInitialPath = initPathfinding.GeneratePath(grid.northPoints);
        List<Node> eastInitialPath =  initPathfinding.GeneratePath(grid.eastPoints);
        List<Node> southInitialPath =  initPathfinding.GeneratePath(grid.southPoints);
        List<Node> westInitialPath =  initPathfinding.GeneratePath(grid.westPoints);

        northPath = resmoothPathfinding.GeneratePath(grid.northPoints, northInitialPath);
        eastPath = resmoothPathfinding.GeneratePath(grid.eastPoints, eastInitialPath);
        southPath = resmoothPathfinding.GeneratePath(grid.southPoints, southInitialPath);
        westPath = resmoothPathfinding.GeneratePath(grid.westPoints, westInitialPath);

        wfc.StartWave(grid.NodeFromPos(northPath[Random.Range(0, northPath.Length)]));
    }

    public Vector3[] GetPath(NodeQuadrant quadrant)
    {
        if (quadrant == NodeQuadrant.North) return northPath;
        else if (quadrant == NodeQuadrant.East) return eastPath;
        else if (quadrant == NodeQuadrant.South) return southPath;
        return westPath;
    }
}
