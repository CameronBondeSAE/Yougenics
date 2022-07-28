using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ollie
{
    public class AStar : MonoBehaviour
    {
        public LevelManager lm;
        //public Heap<WaterNode> openPathNodes;
        public List<WaterNode> closedPathNodes;
        public WaterNode startLocation;
        public WaterNode targetLocation;
        public WaterNode currentLocation;
        public List<WaterNode> pathfindingUnblockedNodes;
        //public CritterAI critter;
        //public CritterAIPlanner targetCritter;
        public List<Vector3> vector3Path;
        public bool active;
        public float timer;

        //only keep this for visualizing path creation
        public float pathDelay;
        public iPathable parent;

        private void Awake()
        {
            parent = GetComponentInParent<iPathable>();
        }

        private void Start()
        {
            closedPathNodes = new List<WaterNode>();
            pathfindingUnblockedNodes = new List<WaterNode>();
            //vector3Path = new List<Vector3>();
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
            //startLocation = critter.currentLocation;
            //targetLocation = targetCritter.currentLocation;
            startLocation.startLocation = true;
            targetLocation.targetLocation = true;
        }

        private void Update()
        {
            // timer += Time.deltaTime;
            // if (timer > 2)
            // {
            //     timer = 0;
            //     if (active)
            //     {
            //         AStarPathfindingStart();
            //         //FindPath(startPos,targetPos);
            //     }
            // }
            //
        }

        public void FindPath(Vector3 startPos, Vector3 targetPos)
        {
            parent.ClearPath();
            
            //to show path generation, comment out FindPathFunction and uncomment FindPathCoroutine
            //to enable instant path, comment out Coroutine and uncomment Function
            
            //StartCoroutine(FindPathCoroutine());
            FindPathFunction(startPos,targetPos);
        }

        public void FindPathFunction(Vector3 startPos, Vector3 targetPos)
        {
            //might need a fake "tick" via coroutine to slow this down
            // if (startLocation != targetLocation)
            // {
                //startLocation.isOpen = true;
                Heap<WaterNode> openPathNodes = new Heap<WaterNode>(lm.MaxSize);

                WaterNode startNode = lm.ConvertToGrid(startPos);
                WaterNode targetNode = lm.ConvertToGrid(targetPos);
                openPathNodes.Add(startNode);

                while (openPathNodes.Count > 0)
                {
                    WaterNode currentNode = openPathNodes.RemoveFirst();
                    closedPathNodes.Add(currentNode);
                    //i = 1 because we've already assigned the starting node to address 0
                    // for (int i = 1; i < openPathNodes.Count; i++)
                    // {
                    //     //if the f cost is less than current location OR if it's the same and it's H COST is lower
                    //     //then make that the new current location
                    //     if (openPathNodes[i].fCost < currentLocation.fCost ||
                    //         openPathNodes[i].fCost == currentLocation.fCost &&
                    //         openPathNodes[i].hCost < currentLocation.hCost)
                    //     {
                    //         currentLocation = openPathNodes[i];
                    //     }
                    // }
                
                    //since we're now at a new current location, it's now closed
                    //so remove from Open and add to Closed
                    //openPathNodes.Remove(currentLocation);
                    //closedPathNodes.Add(currentLocation);
                    

                    //exit here if we're now at our desired destination
                    if (currentNode == targetNode)
                    {
                        CreatePath(startNode,targetNode);
                        return;
                    }

                    //check all neighbours of current node
                    foreach (WaterNode neighbour in currentNode.neighbours)
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
                        int newCostToNeighbour = currentNode.gCost + GetDistance(currentNode, neighbour);
                        if (newCostToNeighbour < neighbour.gCost || !openPathNodes.Contains(neighbour))
                        {
                            //current gCost is best so far, so neighbour's new gCost is cheapest cost from current to neighbour, + current cost
                            neighbour.gCost = newCostToNeighbour;
                            neighbour.hCost = GetDistance(neighbour, targetNode);
                            neighbour.parent = currentNode;
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
                startLocation.isOpen = true;

                Heap<WaterNode> openPathNodes = new Heap<WaterNode>(lm.MaxSize);
                WaterNode startNode = lm.ConvertToGrid(startPos);
                WaterNode targetNode = lm.ConvertToGrid(targetPos);
                openPathNodes.Add(startNode);

                while (openPathNodes.Count > 0)
                {
                    WaterNode currentNode = openPathNodes.RemoveFirst();
                    closedPathNodes.Add(currentNode);
                    //i = 1 because we've already assigned the starting node to address 0
                    // for (int i = 1; i < openPathNodes.Count; i++)
                    // {
                    //     //if the f cost is less than current location OR if it's the same and it's H COST is lower
                    //     //then make that the new current location
                    //     if (openPathNodes[i].fCost < currentLocation.fCost ||
                    //         openPathNodes[i].fCost == currentLocation.fCost &&
                    //         openPathNodes[i].hCost < currentLocation.hCost)
                    //     {
                    //         currentLocation = openPathNodes[i];
                    //     }
                    // }
                
                    //since we're now at a new current location, it's now closed
                    //so remove from Open and add to Closed
                    //openPathNodes.Remove(currentLocation);
                    //closedPathNodes.Add(currentLocation);
                    

                    //exit here if we're now at our desired destination
                    if (currentNode == targetNode)
                    {
                        CreatePath(startNode,targetNode);
                        break;
                    }

                    //check all neighbours of current node
                    foreach (WaterNode neighbour in currentNode.neighbours)
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
                        int newCostToNeighbour = currentNode.gCost + GetDistance(currentNode, neighbour);
                        if (newCostToNeighbour < neighbour.gCost || !openPathNodes.Contains(neighbour))
                        {
                            //current gCost is best so far, so neighbour's new gCost is cheapest cost from current to neighbour, + current cost
                            neighbour.gCost = newCostToNeighbour;
                            neighbour.hCost = GetDistance(neighbour, targetNode);
                            neighbour.parent = currentNode;
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

                yield return null;
                //}
                // else
                // {
                //     startLocation = targetLocation;
                //     targetLocation = pathfindingUnblockedNodes[UnityEngine.Random.Range(0, pathfindingUnblockedNodes.Count)];
                // }
        }

        void CreatePath(WaterNode startNode, WaterNode endNode)
        {
            //vector3Path.Clear();
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
                
                //critter.path.Add(lm.ConvertToWorld(node));
                
                //something like get generic parent to interface iPathable
                //so they all have GeneratePath(WaterNode node)
                //parent.GeneratePath(node)
                //each parent has List<Vector3> path
                //GeneratePath(WaterNode node)
                //path.Add(lm.ConvertToWorld(node));
                
                parent.GeneratePath(node);
            }
            
            //openPathNodes.Clear();
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
        }
    }
}