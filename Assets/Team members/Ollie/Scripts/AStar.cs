using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace Ollie
{
    public class AStar : MonoBehaviour
    {
        public LevelManager lm;
        private PathManager pathManager;
        public Heap<WaterNode> openPathNodes;
        public List<WaterNode> closedPathNodes;
        public WaterNode startLocation;
        public WaterNode targetLocation;
        public WaterNode currentLocation;
        public List<WaterNode> pathfindingUnblockedNodes;
        public CritterAI critter;
        public CritterAIPlanner targetCritter;
        public List<Vector3> vector3Path;
        public bool active;
        public float timer;

        //only keep this for visualizing path creation
        public float pathDelay;

        private void Awake()
        {
            pathManager = GetComponent<PathManager>();
        }

        private void Start()
        {
            closedPathNodes = new List<WaterNode>();
            pathfindingUnblockedNodes = new List<WaterNode>();
            vector3Path = new List<Vector3>();
            active = false;
        }

        public void AStarPathfindingStart()
        {
            foreach (WaterNode node in lm.gridNodeReferences)
            {
                node.isPath = false;
                node.startLocation = false;
                node.targetLocation = false;
            }
            //openPathNodes.Clear();
            closedPathNodes.Clear();
            
            //this is expensive and I should scale it down somehow
            for (int x = 0; x < lm.sizeX; x++)
            {
                for (int z = 0; z < lm.sizeZ; z++)
                {
                    if (!lm.gridNodeReferences[x, z].isBlocked)
                    {
                        pathfindingUnblockedNodes.Add(lm.gridNodeReferences[x,z]);
                    }
                }
            }

            WaterNode random1 = pathfindingUnblockedNodes[UnityEngine.Random.Range(0, pathfindingUnblockedNodes.Count)];
            WaterNode random2 = pathfindingUnblockedNodes[UnityEngine.Random.Range(0, pathfindingUnblockedNodes.Count)];
            startLocation = critter.currentLocation;
            targetLocation = targetCritter.currentLocation;
            startLocation.startLocation = true;
            targetLocation.targetLocation = true;
        }

        private void Update()
        {
            timer += Time.deltaTime;
            if (timer > 0.5f)
            {
                timer = 0;
                if (active)
                {
                    AStarPathfindingStart();
                    FindPath();
                }
            }
            
        }

        public void StartFindPath(Vector3 startPos, Vector3 targetPos)
        {
            critter.path.Clear();
            StartCoroutine(FindPathCoroutine(startPos, targetPos));
        }

        public void FindPath()
        {
            critter.path.Clear();
            
            //to show path generation, comment out FindPathFunction and uncomment FindPathCoroutine
            //to enable instant path, comment out Coroutine and uncomment Function
            
            //StartCoroutine(FindPathCoroutine());
            FindPathFunction();
        }

        public void FindPathFunction()
        {
            //might need a fake "tick" via coroutine to slow this down
            // if (startLocation != targetLocation)
            // {
                startLocation.isOpen = true;
                
                openPathNodes = new Heap<WaterNode>(lm.MaxSize);
                openPathNodes.Add(startLocation);

                while (openPathNodes.Count > 0)
                {
                    currentLocation = openPathNodes.RemoveFirst();
                    closedPathNodes.Add(currentLocation);
                    

                    //exit here if we're now at our desired destination
                    if (currentLocation == targetLocation)
                    {
                        CreatePath(startLocation,targetLocation);
                        return;
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
                }
            //}
            // else
            // {
            //     startLocation = targetLocation;
            //     targetLocation = pathfindingUnblockedNodes[UnityEngine.Random.Range(0, pathfindingUnblockedNodes.Count)];
            // }
        }
        public IEnumerator FindPathCoroutine(Vector3 startPos, Vector3 targetPos)
        {
            //might need a fake "tick" via coroutine to slow this down
            // if (startLocation != targetLocation)
            // {
                

                Vector3[] waypoints = Array.Empty<Vector3>();
                bool pathSuccess = false;

                WaterNode startNode = lm.ConvertToGrid(startPos);
                WaterNode targetNode = lm.ConvertToGrid(targetPos);
                
                startNode.isOpen = true;

                if (!startNode.isBlocked && !targetNode.isBlocked)
                {
                    openPathNodes = new Heap<WaterNode>(lm.MaxSize);
                    openPathNodes.Add(startNode);

                    while (openPathNodes.Count > 0)
                    {
                        currentLocation = openPathNodes.RemoveFirst();
                        closedPathNodes.Add(currentLocation);
                        

                        //exit here if we're now at our desired destination
                        if (currentLocation == targetNode)
                        {
                            pathSuccess = true;
                            //NOTE: Switch these two depending on if function or coroutine
                            //return;
                            break;
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
                                neighbour.hCost = GetDistance(neighbour, targetNode);
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
                    }
                }
                yield return null;
                if (pathSuccess)
                {
                    waypoints = CreatePath(startNode,targetNode);
                }
                pathManager.FinishedProcessingPath(waypoints,pathSuccess);
                //}
                // else
                // {
                //     startLocation = targetLocation;
                //     targetLocation = pathfindingUnblockedNodes[UnityEngine.Random.Range(0, pathfindingUnblockedNodes.Count)];
                // }
        }

        Vector3[] CreatePath(WaterNode startNode, WaterNode endNode)
        {
            vector3Path.Clear();
            List<WaterNode> path = new List<WaterNode>();
            WaterNode currentNode = endNode;

            while (currentNode != startNode)
            {
                path.Add(currentNode);
                currentNode = currentNode.parent;
            }

            Vector3[] waypoints = SimplifyPath(path);
            Array.Reverse(waypoints);

            foreach (WaterNode node in path)
            {
                node.isPath = true; 
                critter.path.Add(lm.ConvertToWorld(node));
            }
            //openPathNodes.Clear();
            closedPathNodes.Clear();
            return waypoints;
        }

        Vector3[] SimplifyPath(List<WaterNode> path)
        {
            List<Vector3> waypoints = new List<Vector3>();
            Vector2 directionOld = Vector2.zero;
            for (int i = 1; i < path.Count; i++)
            {
                Vector2 directionNew = new Vector2(path[i - 1].levelManager.sizeX - path[i].levelManager.sizeX,
                    path[i - 1].levelManager.sizeZ - path[i].levelManager.sizeZ);
                if (directionNew != directionOld)
                {
                    waypoints.Add(path[i].levelManager.ConvertToWorld(path[i]));
                }

                directionOld = directionNew;
            }

            return waypoints.ToArray();
        }
        
        
        int GetDistance(WaterNode nodeA, WaterNode nodeB)
        {
            //gets vertical and horizontal distance between nodes
            int distanceX = Mathf.Abs(nodeA.gridPosition.x - nodeB.gridPosition.x);
            int distanceY = Mathf.Abs(nodeA.gridPosition.y - nodeB.gridPosition.y);

            //14 is the diagonal cost to move, 10 is the cost to move in a cardinal direction
            if (distanceX > distanceY) return 14 * distanceY + 10 * (distanceX - distanceY);
            else return 14 * distanceX + 10 * (distanceY - distanceX);
        }
    }
}