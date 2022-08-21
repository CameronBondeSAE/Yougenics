using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    public bool isBlocked;
    public Vector2 gridPos;

    public int gCost;
    public int hCost;

    public int fCost
    {
        get
        {
            return gCost + hCost;
        }
    }

    public Node parent;
}

public class WorldScanner : MonoBehaviour
{
    //Singleton Declaration
    public static WorldScanner instance;

    //General Variables
    public Node startNode, endNode;
    public Vector2 gridSize;
    public Node[,] gridNodeReferences;
    public LayerMask obstacle;
    float height = 0;

    [Header("Option")]
    public bool autoGenerateGridOnAwake = false;
    public bool constantScan = false;

    //List for visualizing nodes only
    public List<Node> path = new List<Node>();
    public List<Node> openList = new List<Node>();
    public List<Node> closedList = new List<Node>();
    public List<Node> totalOpenNodes = new List<Node>();

    //Used for storing the closed nodes of an object on the map once it moves (so we can rescan those nodes and update them)
    List<Node> objectClosedNodes = new List<Node>();

  //LevelInfo level;

    public float nodeSize = 2f;
    public bool debugGizmos = false;

    private void Awake()
    {
        instance = this;

        //TODO: Change to listen to event from game manager
        /*level = FindObjectOfType<LevelInfo>();

        if (level != null)
        {
            gridSize.x = Mathf.RoundToInt(level.bounds.extents.x / nodeSize) + 1;
            gridSize.y = Mathf.RoundToInt(level.bounds.extents.z / nodeSize) + 1;
        }*/

        if (autoGenerateGridOnAwake)
            CreateGrid();
    }

    void Update()
    {
        if (constantScan)
        {
            CreateGrid();
        }
    }

    public void CreateGrid()
    {
        gridNodeReferences = new Node[(int)gridSize.x, (int)gridSize.y];

        for (int x = 0; x < gridSize.x; x++)
        {
            for (int y = 0; y < gridSize.y; y++)
            {
                //For each grid position, create a new node and store node position
                gridNodeReferences[x, y] = new Node();
                gridNodeReferences[x, y].gridPos = new Vector2(x * nodeSize, y * nodeSize);

                //Check for obstacle
                if (Physics.CheckBox(new Vector3(x * nodeSize, height + 50f, y * nodeSize), new Vector3(0.5f, 50f, 0.5f), Quaternion.identity, obstacle))
                {
                    // Something is there
                    gridNodeReferences[x, y].isBlocked = true;
                }
                else
                {
                    totalOpenNodes.Add(gridNodeReferences[x, y]);
                }
            }
        }
    }

    void CheckForBlockedNeighbours(Node node)
    {
        foreach (Node neighbour in GetNeighbours(node))
        {
            if (neighbour.isBlocked && !objectClosedNodes.Contains(neighbour))
            {
                objectClosedNodes.Add(neighbour);
            }
        }
    }

    public Node WorldToNodePos(Vector3 worldPos)
    {
        int x;
        int y;

        if ((worldPos.x <= gridSize.x * nodeSize && worldPos.x >= 0) && (worldPos.z <= gridSize.y * nodeSize && worldPos.z >= 0))
        {
            x = (int)(worldPos.x / nodeSize);
            y = (int)(worldPos.z / nodeSize);
            return gridNodeReferences[x, y];

        }
        else
        {
            Debug.Log("Object World Position is outside of Grid Space");
            return null;
        }
    }

    public Vector2 NodeToWorldPos(Node node)
    {
        //if(node.gridPos.x <= gridSize.x * nodeSize && node.gridPos.x >= 0 && node.gridPos.y <= gridSize.y * nodeSize && node.gridPos.y >= 0)
        if (node.gridPos != null)
        {
            float x = node.gridPos.x;
            float y = node.gridPos.y;
            return new Vector2(x, y);
        }
        else
        {
            Debug.Log("Null Node Error");
            return Vector2.zero;
        }
    }

    public List<Node> GetNeighbours(Node node)
    {
        List<Node> neighbours = new List<Node>();

        //Check in a 3 by 3 grid for all neighbours
        for (int x = -1; x < 2; x++)
        {
            for (int y = -1; y < 2; y++)
            {
                //we are the centre grid - skip us
                if (x == 0 && y == 0)
                {
                    continue;
                }
                else
                {
                    //check to make neighbour is within the grid
                    //int checkX = (int)node.gridPos.x + x;
                    //int checkY = (int)node.gridPos.y + y;
                    if (node != null)
                    {
                        if (((int)node.gridPos.x + x >= 0 && (int)node.gridPos.x + x <= gridSize.x - 1) && ((int)node.gridPos.y + y >= 0 && (int)node.gridPos.y + y <= gridSize.y - 1))
                        {
                            neighbours.Add(gridNodeReferences[(int)node.gridPos.x + x, (int)node.gridPos.y + y]);
                        }
                        else
                        {
                            continue;
                        }
                    }
                }
            }
        }

        return neighbours;
    }

    void RemoveNodeFromOpenNodeList(Node node)
    {
        if (totalOpenNodes.Contains(node))
        {
            totalOpenNodes.Remove(node);
        }
    }

    void AddNodeToOpenNodeList(Node node)
    {
        if (!totalOpenNodes.Contains(node))
        {
            totalOpenNodes.Add(node);
        }
    }

    private void OnDrawGizmos()
    {
        if (!debugGizmos)
        {
            return;
        }

        //Stop constant null errors when not in play mode using null check
        if (gridNodeReferences != null)
        {
            // if(startNode != null)
            // startNode = WorldToNodePos(startPos.position);
            //endNode = WorldToNodePos(endPos.position);

            //loop through each node and draw a cube for each grid position
            for (int x = 0; x < gridSize.x; x++)
            {
                for (int y = 0; y < gridSize.y; y++)
                {

                    //change colour to indicate if path is blocked or clear
                    if (gridNodeReferences[x, y] == startNode)
                    {
                        Gizmos.color = Color.cyan;
                    }
                    else if (gridNodeReferences[x, y] == endNode)
                    {
                        Gizmos.color = Color.grey;
                    }
                    else if (gridNodeReferences[x, y].isBlocked)
                    {
                        Gizmos.color = Color.red;
                    }
                    else if (gridNodeReferences[x, y] != startNode)
                    {
                        Gizmos.color = Color.green;
                    }

                    if (path != null)
                    {
                        if (path.Contains(gridNodeReferences[x, y]) && gridNodeReferences[x, y] != endNode)
                        {
                            Gizmos.color = Color.black;
                        }
                    }

                    if (openList != null)
                    {
                        if (openList.Contains(gridNodeReferences[x, y]) && gridNodeReferences[x, y] != endNode && gridNodeReferences[x, y] != startNode)
                        {
                            Gizmos.color = Color.white;
                        }
                    }

                    if (closedList != null)
                    {
                        if (closedList.Contains(gridNodeReferences[x, y]) && gridNodeReferences[x, y] != endNode && gridNodeReferences[x, y] != startNode)
                        {
                            Gizmos.color = Color.blue;
                        }
                    }

                    //Draw after to prevent weird offset
                    Gizmos.DrawCube(new Vector3(x * nodeSize, height, y * nodeSize), Vector3.one * nodeSize);

                }
            }
        }
    }
}
