using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kevin
{
    public class AStarManager : MonoBehaviour
    {
        private NodeClass[,] gridNodes;

        [SerializeField] private Vector2 gridWorldSize;
        [SerializeField] private float nodeRadius;
        [SerializeField] private float nodeDiameter;
        [SerializeField] private Vector3 worldBottomLeft;
        private Vector3 _gridTileSize;
        private int gridSizeX, gridSizeZ;


        public Vector2Int startPosition;
        public Vector2Int endPosition;
        public Vector2Int currentPosition;
        
        public NodeClass currentNode;
        public NodeClass endNode;
        
        public List<NodeClass> openedNode = new();
        public List<NodeClass> pathNode = new();
        
        public LayerMask obstacle;

        public Transform start;
        public Transform end;
        public void CreateGrid()
        {
            gridNodes = new NodeClass[gridSizeX, gridSizeZ];
            
            _gridTileSize = new Vector3(gridSizeX/gridWorldSize.x, 1, gridSizeZ/gridWorldSize.y);
            
            worldBottomLeft = transform.position - Vector3.right * gridWorldSize.x / 2 -
                                      Vector3.forward * gridWorldSize.y / 2; 
            
            for (int x = 0; x < gridSizeX; x++)
            {
                for (int z = 0; z < gridSizeZ; z++)
                {
                    Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) +
                                         Vector3.forward * (z * nodeDiameter + nodeRadius);
                    bool isBlocked = (Physics.CheckSphere(worldPoint, nodeRadius,obstacle));
                    gridNodes[x, z] = new NodeClass(worldPoint,isBlocked, new Vector2Int(x,z));
                    
                    /*gridNodes[x,z] = new NodeClass();
                    Vector3 worldPoint = new Vector3(0+x, 0, 0+z);
                    gridNodes[x, z].worldPosition = worldPoint;
                    if (Physics.OverlapBox(gridNodes[x,z].worldPosition, (_gridTileSize * 0.5f), Quaternion.identity, obstacle)
                            .Length != 0)
                    {
                        gridNodes[x, z].isBlocked = true;
                    }*/
                }
            }
        }
       // + _gridTileSize.x / 2f
        public Vector2Int ConvertWorldPositionIndex(Vector3 position)
        {
            int xIndex = Mathf.RoundToInt((position.x - _gridTileSize.x / 2f - worldBottomLeft.x) / _gridTileSize.x);
            int yIndex = Mathf.RoundToInt((position.z - _gridTileSize.z / 2f - worldBottomLeft.z) / _gridTileSize.z);

            return new Vector2Int(xIndex, yIndex);
        }
        
        public Vector3 ConvertWorldPositionIndex(Vector2Int index)
        {
            float xCoord = index.x * _gridTileSize.x + 0.5f * _gridTileSize.x + worldBottomLeft.x; 
            //float xCoord = (index.x * _gridTileSize.x) / (worldBottomLeft.x + _gridTileSize.x * gridSizeX) - worldBottomLeft.x; 
            //float zCoord = (index.y * _gridTileSize.z) / (worldBottomLeft.z + _gridTileSize.z * gridSizeZ) - worldBottomLeft.z; 
            float zCoord = index.y * _gridTileSize.z + 0.5f * _gridTileSize.z + worldBottomLeft.z; 
            return new Vector3(xCoord, 1, zCoord);
        }
        
        public void Update()
        {
            startPosition = ConvertWorldPositionIndex(start.position);
            endPosition = ConvertWorldPositionIndex(end.position);
        }
        public void AStarAlgorithm()
        {
            //currentNode = currentNode[startPosition.x, startPosition.y]; 
            //pathfinding code here
        }
        public void CheckNeighbours()
        {
            for (int x = -1; x < 2; x++)
            {
                for (int y = -1; y < 2; y++)
                {
                    gridNodes[x, y] = gridNodes[currentPosition.x + x, currentPosition.y + y];
                    if (!(gridNodes[x, y].isBlocked))
                    {
                        openedNode.Add(gridNodes[x,y]);
                    }
                    //gridNodes[currentPosition.x + neighbourX, currentPosition.y]
                    //openedNode.Add(gridNodes[x, y]);
                }
            }
        }

        /*public void PathfindingStart()
        {
            currentNode = startNode;
            currentPosition = currentNode.gridPosition;
            CheckNeighbours();
        }*/

        public void Restart()
        {
            
        }
        public Vector2Int RandomiseStartLocation()
        {
            startPosition = new Vector2Int(Random.Range(0,gridSizeX), Random.Range(0,gridSizeZ));
            if (gridNodes[startPosition.x, startPosition.y].isBlocked || startPosition == endPosition)
            {
                RandomiseStartLocation();
            }
            return startPosition;
        }
        
        public Vector2Int RandomiseEndLocation()
        {
            endPosition = new Vector2Int(Random.Range(0,gridSizeX), Random.Range(0,gridSizeZ));
            if (gridNodes[startPosition.x, startPosition.y].isBlocked || startPosition == endPosition)
            {
                RandomiseEndLocation();
            }
            return endPosition;
        }
        public void Awake()
        {
            nodeDiameter = nodeRadius * 2f;
            gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
            gridSizeZ = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);
            CreateGrid();
        }
        
        void OnDrawGizmos()
        {
            Gizmos.DrawWireCube(transform.position,new Vector3(gridSizeX, 1, gridSizeZ));
            
            
                for (int x = 0; x < gridSizeX; x++)
                {
                    for (int y = 0; y < gridSizeZ; y++)
                    {
                        if (gridNodes != null)
                        {
                            if (gridNodes[x, y] == currentNode)
                            {
                                Gizmos.color = Color.blue;
                                Gizmos.DrawCube(gridNodes[x,y].worldPosition, Vector3.one * (nodeDiameter - 0.1f));
                            }
                            else if (gridNodes[x, y] == endNode)
                            {
                                Gizmos.color = Color.yellow;
                                Gizmos.DrawCube(gridNodes[x,y].worldPosition, Vector3.one * (nodeDiameter - 0.1f));
                            }
                            else if (openedNode.Contains(gridNodes[x,y]))
                            {
                                Gizmos.color = Color.green;
                                Gizmos.DrawCube(gridNodes[x,y].worldPosition,Vector3.one * (nodeDiameter - 0.1f));
                            }
                            /*else if (gridNodes[x, y].isClosed)
                            {
                                Gizmos.color = Color.red;
                                Gizmos.DrawCube(gridNodes[x,y].worldPosition,Vector3.one * (nodeDiameter - 0.1f));
                            }*/
                            else if (gridNodes[x, y].isBlocked)
                            {
                                Gizmos.color = Color.black;
                                Gizmos.DrawCube(gridNodes[x,y].worldPosition, Vector3.one * (nodeDiameter - 0.1f));
                            }
                            else 
                            {
                                Gizmos.color = Color.white;
                                Gizmos.DrawCube(gridNodes[x,y].worldPosition, Vector3.one * (nodeDiameter - 0.1f));
                            }
                            
                            if (x == startPosition.x && y == startPosition.y)
                            {
                                Gizmos.color = Color.red;
                                Gizmos.DrawCube(gridNodes[x,y].worldPosition, Vector3.one * (nodeDiameter - 0.1f));
                            } 
                            
                            if (x == endPosition.x && y == endPosition.y)
                            {
                                Gizmos.color = Color.cyan;
                                Gizmos.DrawCube(gridNodes[x,y].worldPosition, Vector3.one * (nodeDiameter - 0.1f));
                            }
                            
                            

                        }
                    }
                /*foreach (NodeClass n in gridNodes) 
                {
                        Gizmos.color = (n.isBlocked) ? Color.white : Color.red;
                        if (n[gridSizeX,gridSizeY] == startPosition)
                        {
                            
                        }
                        Gizmos.DrawCube(n.worldPosition, Vector3.one * (nodeDiameter - 0.1f));
                }*/
                
                /*if (gridNodes[x, y].isBlocked)
                {
                    Gizmos.color = Color.red;
                    Gizmos.DrawCube(new Vector3(x, 0, y), Vector3.one);
                }
                else
                {
                    Gizmos.color = Color.white;
                    Gizmos.DrawCube(new Vector3(x, 0, y), Vector3.one);
                }*/
            }
        }
    }
}

