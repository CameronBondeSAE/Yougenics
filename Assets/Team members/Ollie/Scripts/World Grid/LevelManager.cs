using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.VisualScripting;
using UnityEngine;

namespace Ollie
{
    public class LevelManager : MonoBehaviour
    {
        public static LevelManager instance;
        
        public Bounds bounds  = new Bounds(Vector3.zero, new Vector3(80,0,80));
        public WaterNode[,] gridNodeReferences;
        public Vector3 gridTileHalfExtents = new(0.5f, 0.5f, 0.5f);
        public int sizeX;
        public int sizeZ;
        public int lengthX;
        public int lengthZ;
        public int offsetX;
        public int offsetZ;
        public Vector2 currentPosition;
        public LayerMask layers;
        public GameObject waterCube;
        
        //public AStar aStar;
        
        public List<WaterNode> blockedNodes;
        //public List<WaterNode> allNodes;

        public List<Transform> testList;
        public List<WaterNode> openNodeV3List;
        public List<WaterNode> activeWaterNodes;
        public List<Vector3> waterPosList;
        
        public List<WaterNode> temporaryOpen;
        public List<WaterNode> openNodesToAdd;
        public List<WaterNode> openWaterNodes;
        public List<WaterNode> closedWaterNodes;
        public List<WaterNode> pathfindingUnblockedNodes;
        public List<WaterNode> openPathNodes;
        public List<WaterNode> closedPathNodes;
        public WaterNode startLocation;
        public WaterNode targetLocation;
        public WaterNode currentLocation;
        private bool worldInitialised;

        private void OnEnable()
        {
            Utilities.WorldObstacleUpdatedEvent += ScanChunk;
        }

        private void OnDisable()
        {
            Utilities.WorldObstacleUpdatedEvent -= ScanChunk;
        }

        void Awake()
        {
            LevelManager.instance = this;
            
            sizeX = Mathf.RoundToInt(bounds.extents.x) + 1;
            sizeZ = Mathf.RoundToInt(bounds.extents.z) + 1;
            lengthX = Mathf.RoundToInt(bounds.center.x - bounds.extents.x/2);
            lengthZ = Mathf.RoundToInt(bounds.center.z - bounds.extents.z/2);
            offsetX = Mathf.RoundToInt((bounds.center.x));
            offsetZ = Mathf.RoundToInt((bounds.center.z));
            gridNodeReferences = new WaterNode[sizeX, sizeZ];
            blockedNodes = new List<WaterNode>();
            openNodeV3List = new List<WaterNode>();
            activeWaterNodes = new List<WaterNode>();
            temporaryOpen = new List<WaterNode>();
            waterPosList = new List<Vector3>();
            openNodesToAdd = new List<WaterNode>();
            openWaterNodes = new List<WaterNode>();
            closedWaterNodes = new List<WaterNode>();
            pathfindingUnblockedNodes = new List<WaterNode>();
            openPathNodes = new List<WaterNode>();
            closedPathNodes = new List<WaterNode>();
            worldInitialised = false;
            StartCoroutine(ScanWorld());
            //StartCoroutine(UpdateWorld());
        }

        
        private IEnumerator UpdateWorld()
        {
            while (true)
            {
                yield return new WaitForSeconds(5f);
                //if(worldInitialised) ScanWorld();
            }
        }

        public int MaxSize
        {
            get
            {
                return lengthX * lengthZ;
            }
        }

        public WaterNode ConvertToGrid(Vector3 position)
        {
            //outside bounds errors because position sent in can be negative
            //but gridNodeReferences[,] cannot be negative!
            int       positionX = (int)position.x-lengthX;
            int       positionZ = (int)position.z-lengthZ;

            WaterNode node = null;
            
            // HACK CAM: to just return the edge of the map if you try to access past it
            if (positionX > gridNodeReferences.GetLength(0)-1)
            {
                positionX = gridNodeReferences.GetLength(0)-1;
            }
            if (positionZ > gridNodeReferences.GetLength(1)-1)
            {
                positionZ = gridNodeReferences.GetLength(1)-1;
            }
            node = gridNodeReferences[positionX,positionZ];
            return node;
        }

        public Vector3 ConvertToWorld(WaterNode node)
        {
            Vector3 position = node.gridPosVector3;
            return position;
        }

        public void ScanChunk(GameObject go, Bounds objectBounds)
        {
            int minX = ConvertToGrid(objectBounds.min).xPosInArray - 2;
            int maxX = ConvertToGrid(objectBounds.max).xPosInArray + 2;
            int minZ = ConvertToGrid(objectBounds.min).zPosInArray - 2;
            int maxZ = ConvertToGrid(objectBounds.max).zPosInArray + 2;

            for (int x = minX; x < maxX; x++)
            {
                for (int z = minZ; z < maxZ; z++)
                {
                    gridNodeReferences[x,z].ScanMyself();
                }
            }
        }

        private IEnumerator ScanWorld()
        {
            if (worldInitialised == false)
            {
                int nodeCount = 0;
                for (int x = 0; x < sizeX; x++)
                {
                    for (int z = 0; z < sizeZ; z++)
                    {
                        gridNodeReferences[x, z] = new WaterNode();
                        gridNodeReferences[x, z].xPosInArray = x;
                        gridNodeReferences[x, z].zPosInArray = z;
                        gridNodeReferences[x, z].gridPosition = new Vector2Int(x+lengthX, z+lengthZ);

                        var vector3 = new Vector3(lengthX+x, 0, lengthZ+z); 
                        
                        if (Physics.OverlapBox(vector3, gridTileHalfExtents,
                            Quaternion.identity,layers).Length != 0)
                        {
                            gridNodeReferences[x, z].isBlocked = true;
                            blockedNodes.Add(gridNodeReferences[x,z]);
                        }
                        
                        nodeCount += 1;
                        if (nodeCount > 500)
                        {
                            nodeCount = 0;
                            yield return new WaitForNextFrameUnit();
                        }
                    }
                }
                worldInitialised = true;
                AssignNeighbours();
            }

            if (worldInitialised)
            {
                foreach (WaterNode node in gridNodeReferences)
                {
                    node.ScanMyself();
                }
            }
        }

        public void AssignNeighbours()
        {
            for (int x = 0; x < sizeX; x++)
            {
                for (int z = 0; z < sizeZ; z++)
                {
                    if (x > 0) gridNodeReferences[x-1, z].neighbours[2,1] = gridNodeReferences[x, z];
                    if (z > 0) gridNodeReferences[x, z-1].neighbours[1,2] = gridNodeReferences[x, z];
                    if (x < sizeX-1) gridNodeReferences[x+1, z].neighbours[0,1] = gridNodeReferences[x, z];
                    if (z < sizeZ-1) gridNodeReferences[x, z+1].neighbours[1,0] = gridNodeReferences[x, z];
                    if (x > 0 && z > 0) gridNodeReferences[x-1, z-1].neighbours[2,2] = gridNodeReferences[x, z];
                    if (x > 0 && z < sizeZ-1) gridNodeReferences[x-1, z+1].neighbours[2,0] = gridNodeReferences[x, z];
                    if (x < sizeX-1 && z > 0) gridNodeReferences[x+1, z-1].neighbours[0,2] = gridNodeReferences[x, z];
                    if (x < sizeX-1 && z < sizeZ-1) gridNodeReferences[x+1, z+1].neighbours[0,0] = gridNodeReferences[x, z];
                }
            }
        }

        public void SpawnWater()
        {
            //can probably move this out to ScanWorld
            for (int x = 0; x < sizeX; x++)
            {
                for (int z = 0; z < sizeZ; z++)
                {
                    if (!gridNodeReferences[x, z].isBlocked)
                    {
                        openNodeV3List.Add(gridNodeReferences[x,z]);
                    }
                }
            }
            // lengthX = Mathf.RoundToInt(bounds.center.x - bounds.extents.x/2);
            // lengthZ = Mathf.RoundToInt(bounds.center.z - bounds.extents.z/2);
            //new Vector3(lengthX+x, 0, lengthZ+z)

            WaterNode randomWaterNode = openNodeV3List[UnityEngine.Random.Range(0, openNodeV3List.Count)];
            randomWaterNode.isWater = true;
            openWaterNodes.Add(randomWaterNode);
            
            // activeWaterNodes.Add(randomWaterNode);
            // Vector2Int transformPosition = randomWaterNode.gridPosition;
            
            // GameObject go = Instantiate(waterCube);
            // go.transform.position = new Vector3(transformPosition.x+lengthX,1,transformPosition.y+lengthZ);
            // waterPosList.Add(go.transform.position);
            }

        // public void CheckNeighbours()
        // {
        //     temporaryOpen = openWaterNodes;
        //     foreach (WaterNode waterNode in temporaryOpen)
        //     {
        //         waterNode.CheckNeighbours();
        //     }
        //     temporaryOpen.Clear();
        //     openWaterNodes.Clear();
        // }

        public void AddNodes()
        {
            foreach (WaterNode waterNode in openNodesToAdd)
            {
                if(!openWaterNodes.Contains(waterNode)) openWaterNodes.Add(waterNode);
            }
            openNodesToAdd.Clear();
        }

        public void FillNeighbours()
        {
            StartCoroutine(FillNeighboursCoroutine());
        }

        public IEnumerator FillNeighboursCoroutine()
        {
            //int count = activeWaterNodes.Count;
            yield return new WaitForSeconds(0.1f);
            // foreach (WaterNode waterNode in activeWaterNodes)
            // {
            //     waterNode.FillNeighbours();
            // }

            // temporaryActiveWaterNodes.Clear();
            // temporaryActiveWaterNodes = activeWaterNodes;
            
            // for (int i = 0; i < count; i++)
            // {
            //     activeWaterNodes[i].FillNeighbours();
            //     activeWaterNodes.Remove(activeWaterNodes[i]);
            // }
            temporaryOpen = openWaterNodes;
            foreach (WaterNode neighbour in temporaryOpen)
            {
                if(!neighbour.isBlocked) neighbour.isWater = true;
                if(!closedWaterNodes.Contains(neighbour)) closedWaterNodes.Add(neighbour);
            }
            
        }

        #region Old Pathfinding Before Separating into it's own script
        /*public void AStarPathfindingStart()
        {
            aStar.AStarPathfindingStart();
            aStar.active = true;
        }

        public void FindPath()
        {
            //aStar.FindPath();
        }

        
        public void AStarPathfindingStart()
        {
            // AStar = !AStar;
            // if (AStar)
            // {
            foreach (WaterNode node in gridNodeReferences)
            {
                node.isPath = false;
                node.startLocation = false;
                node.targetLocation = false;
            }
            openPathNodes.Clear();
            closedPathNodes.Clear();
            
            //this is expensive and I should scale it down somehow
            for (int x = 0; x < sizeX; x++)
                {
                    for (int z = 0; z < sizeZ; z++)
                    {
                        if (!gridNodeReferences[x, z].isBlocked)
                        {
                            pathfindingUnblockedNodes.Add(gridNodeReferences[x,z]);
                        }
                    }
                }

                WaterNode random1 = pathfindingUnblockedNodes[UnityEngine.Random.Range(0, pathfindingUnblockedNodes.Count)];
                WaterNode random2 = pathfindingUnblockedNodes[UnityEngine.Random.Range(0, pathfindingUnblockedNodes.Count)];
                startLocation = random1;
                targetLocation = random2;
                startLocation.startLocation = true;
                targetLocation.targetLocation = true;
                // }
        }

        public void FindPath()
        {
            StartCoroutine(FindPathCoroutine());
        }

        public IEnumerator FindPathCoroutine()
        {
            //might need a fake "tick" via coroutine to slow this down
            // if (startLocation != targetLocation)
            // {
                startLocation.isOpen = true;
                openPathNodes.Add(startLocation);

                while (openPathNodes.Count > 0)
                {
                    currentLocation = openPathNodes[0];
                    //i = 1 because we've already assigned the starting node to address 0
                    for (int i = 1; i < openPathNodes.Count; i++)
                    {
                        //if the f cost is less than current location OR if it's the same and it's H COST is lower
                        //then make that the new current location
                        if (openPathNodes[i].fCost < currentLocation.fCost ||
                            openPathNodes[i].fCost == currentLocation.fCost &&
                            openPathNodes[i].hCost < currentLocation.hCost)
                        {
                            currentLocation = openPathNodes[i];
                        }
                    }
                
                    //since we're now at a new current location, it's now closed
                    //so remove from Open and add to Closed
                    openPathNodes.Remove(currentLocation);
                    closedPathNodes.Add(currentLocation);
                    

                    //exit here if we're now at our desired destination
                    if (currentLocation == targetLocation)
                    {
                        CreatePath(startLocation,targetLocation);

                        //NOTE: Switch these two depending on if function or coroutine
                        //return;
                        yield break;
                    }

                    //check all neighbours of current node
                    foreach (WaterNode neighbour in currentLocation.neighbours)
                    {
                        //basically ignore itself, since it will always be null in it's own neighbour list
                        if (neighbour == null)
                        {
                            continue;
                        }
                        
                        //if the neighbour is blocked or closed, ie. already been checked!
                        //then continue
                        if (neighbour.isBlocked || closedPathNodes.Contains(neighbour))
                        {
                            continue;
                        }

                        //update parents and F cost according to the best distance from start location to current
                        //f cost is not manually set
                        //g cost and h cost are set, and whenever F cost is required, it's automatically calculated on the node
                        int newCostToNeighbour = currentLocation.gCost + GetDistance(currentLocation, neighbour);
                        if (newCostToNeighbour < neighbour.gCost || !openPathNodes.Contains(neighbour))
                        {
                            //current gCost is best so far, so neighbour's new gCost is cheapest cost from current to neighbour, + current cost
                            neighbour.gCost = newCostToNeighbour;
                            neighbour.hCost = GetDistance(neighbour, targetLocation);
                            neighbour.parent = currentLocation;
                            if (!openPathNodes.Contains(neighbour))
                            {
                                openPathNodes.Add(neighbour);
                                
                            }
                        }
                    
                        
                        // node.gCost = Vector2.Distance(node.gridPosition, startLocation.gridPosition);
                        // node.hCost = Vector2.Distance(targetLocation.gridPosition, node.gridPosition);
                        // node.fCost = node.gCost + node.hCost;
                    }
                    yield return new WaitForSeconds(pathDelay);
                }
            //}
            // else
            // {
            //     startLocation = targetLocation;
            //     targetLocation = pathfindingUnblockedNodes[UnityEngine.Random.Range(0, pathfindingUnblockedNodes.Count)];
            // }
        }

        void CreatePath(WaterNode startNode, WaterNode endNode)
        {
            List<WaterNode> path = new List<WaterNode>();
            WaterNode currentNode = endNode;

            while (currentNode != startNode)
            {
                path.Add(currentNode);
                currentNode = currentNode.parent;
            }
            path.Reverse();
            foreach (WaterNode node in path)
            {
                node.isPath = true;
            }
            openPathNodes.Clear();
            closedPathNodes.Clear();
        }
        
        int GetDistance(WaterNode nodeA, WaterNode nodeB)
        {
            //gets vertical and horizontal distance between nodes
            int distanceX = Mathf.Abs(nodeA.gridPosition.x - nodeB.gridPosition.x);
            int distanceY = Mathf.Abs(nodeA.gridPosition.y - nodeB.gridPosition.y);

            //14 is the diagonal cost to move, 10 is the cost to move in a cardinal direction
            if (distanceX > distanceY) return 14 * distanceY + 10 * (distanceX - distanceY);
            else return 14 * distanceX + 10 * (distanceY - distanceX);
        }*/
        #endregion

        public void SpreadToNeighbours(WaterNode waterNode, Vector2Int transformPosition)
        {
            activeWaterNodes.Add(waterNode);
            // GameObject go = Instantiate(waterCube);
            // go.transform.position = new Vector3(transformPosition.x+lengthX,1,transformPosition.y+lengthZ);
            // waterPosList.Add(go.transform.position);
        }

        public void PrintNeighbourGridPos()
        {
            print(activeWaterNodes.Count);
            // foreach (WaterNode waterNode in activeWaterNodes)
            // {
            //     print(waterNode.gridPosition);
            // }
        }

        //commented out so I could push without errors popping up for others
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = new Color(0.5f, 0.5f, 0.5f, 0.5f);
            Gizmos.DrawCube(bounds.center,bounds.extents);
            
            if (!Application.isPlaying)
            {
                
            }

            if(gridNodeReferences != null && worldInitialised) foreach (WaterNode node in gridNodeReferences)
            {
                
                if (node.isBlocked)
                {
                    Gizmos.color = Color.red;
                    //Gizmos.DrawCube(new Vector3(node.gridPosition.x,0,node.gridPosition.y),Vector3.one);
                }

                if (!node.isBlocked && !node.isWater)
                {
                    Gizmos.color = Color.green;
                    //Gizmos.DrawCube(new Vector3(node.gridPosition.x,0,node.gridPosition.y),Vector3.one);
                }

                // if (node.isWater)
                // {
                //     Gizmos.color = Color.blue;
                //     Gizmos.DrawCube(new Vector3(lengthX+node.gridPosition.x,0,lengthZ+node.gridPosition.y),Vector3.one);
                // }

                // if (node.isPath)
                // {
                //     Gizmos.color = Color.black;
                //     Gizmos.DrawCube(new Vector3(lengthX+node.gridPosition.x,0,lengthZ+node.gridPosition.y),Vector3.one);
                // }

                // if (aStar.openPathNodes.Contains(node) && !node.isPath)
                // {
                //     Gizmos.color = Color.magenta;
                //     Gizmos.DrawCube(new Vector3(lengthX+node.gridPosition.x,0,lengthZ+node.gridPosition.y),Vector3.one);
                // }

                // if (aStar.closedPathNodes.Contains(node) && !node.isPath)
                // {
                //     Gizmos.color = Color.gray;
                //     Gizmos.DrawCube(new Vector3(lengthX+node.gridPosition.x,0,lengthZ+node.gridPosition.y),Vector3.one);
                // }

                // if (node.targetLocation || node.startLocation)
                // {
                //     Gizmos.color = Color.yellow;
                //     Gizmos.DrawCube(new Vector3(lengthX+node.gridPosition.x,0,lengthZ+node.gridPosition.y),Vector3.one);
                // }
                

                //double for loop, too expensive
                /*for (int x = 0; x < sizeX; x++)
                {
                    for (int z = 0; z < sizeZ; z++)
                    {
                        if (gridNodeReferences[x, z] != null)
                        {
                            if (gridNodeReferences[x, z].isBlocked)
                            {
                                Gizmos.color = Color.red;
                                Gizmos.DrawCube(new Vector3(lengthX+x,0,lengthZ+z),Vector3.one);
                            }
                        
                            if (!gridNodeReferences[x, z].isBlocked && !gridNodeReferences[x,z].isWater)
                            {
                                Gizmos.color = Color.green;
                                Gizmos.DrawCube(new Vector3(lengthX+x,0,lengthZ+z),Vector3.one);
                        }

                        if (gridNodeReferences[x, z].isWater)
                        {
                            Gizmos.color = Color.blue;
                            Gizmos.DrawCube(new Vector3(lengthX+x,0,lengthZ+z),Vector3.one);
                        }

                        if (gridNodeReferences[x, z].isPath)
                        {
                            Gizmos.color = Color.black;
                            Gizmos.DrawCube(new Vector3(lengthX+x,0,lengthZ+z),Vector3.one);
                        }

                        if (openPathNodes.Contains(gridNodeReferences[x, z]) && !gridNodeReferences[x,z].isPath)
                        {
                            Gizmos.color = Color.magenta;
                            Gizmos.DrawCube(new Vector3(lengthX+x,0,lengthZ+z),Vector3.one);
                        }

                        if (closedPathNodes.Contains(gridNodeReferences[x, z]) && !gridNodeReferences[x,z].isPath)
                        {
                            Gizmos.color = Color.gray;
                            Gizmos.DrawCube(new Vector3(lengthX+x,0,lengthZ+z),Vector3.one);
                        }
                    }
                }
            }
            

            /*if (AStar)
            {
                for (int x = 0; x < sizeX; x++)
                {
                    for (int z = 0; z < sizeZ; z++)
                    {
                        if (gridNodeReferences[x, z] != null)
                        {
                            if (gridNodeReferences[x, z].isBlocked)
                            {
                                Gizmos.color = Color.red;
                                Gizmos.DrawCube(new Vector3(lengthX+x,0,lengthZ+z),Vector3.one);
                            }

                            if (gridNodeReferences[x, z].isClosed)
                            {
                                Gizmos.color = Color.black;
                                Gizmos.DrawCube(new Vector3(lengthX+x,0,lengthZ+z),Vector3.one);
                            }
                    
                            if (!gridNodeReferences[x, z].isOpen)
                            {
                                Gizmos.color = Color.green;
                                Gizmos.DrawCube(new Vector3(lengthX+x,0,lengthZ+z),Vector3.one);
                            }

                            if (gridNodeReferences[x, z].isPath)
                            {
                                Gizmos.color = Color.blue;
                                Gizmos.DrawCube(new Vector3(lengthX+x,0,lengthZ+z),Vector3.one);
                            }
                        }
                    }
                }
            }*/
            }
        }
    }
}