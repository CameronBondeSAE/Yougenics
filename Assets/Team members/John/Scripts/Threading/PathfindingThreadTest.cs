using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Jobs;
using Unity.Collections;

public struct JohnPathfindJob : IJob
{
    Vector3 targetPos;
    Vector3 startPos;
    Node targetNode;
    List<Node> path;

    public void Execute()
    {
        //FindPath();
    }

    public void FindPath(Vector3 _startPos, Vector3 _targetPos)
    {
        //Keep for ReScan Reference
        targetPos = _targetPos;
        startPos = _startPos;

        Node startNode = WorldScanner.instance.WorldToNodePos(_startPos);
        targetNode = WorldScanner.instance.WorldToNodePos(_targetPos);

        List<Node> openList = new List<Node>();
        List<Node> closedList = new List<Node>();

        /*if (visualizeOpenCloseLists)
        {
            WorldScanner.instance.openList = openList;
            WorldScanner.instance.closedList = closedList;
        }
        else
        {
            WorldScanner.instance.openList = null;
            WorldScanner.instance.closedList = null;
        }*/

        openList.Add(startNode);

        while (openList.Count > 0)
        {
            //Initialise a current node
            Node currentNode = openList[0];

            //Once our openlist becomes populated with neighbours
            //Current node SHOULD ONLY be the node with the lowest f cost - check through each node in the list to find the node with the lowest f cost
            for (int i = 1; i < openList.Count; i++)
            {
                //if node in open list f cost is less then current node f cost OR if same f cost, get node with the lowest hCost
                if (openList[i].fCost < currentNode.fCost || openList[i].fCost == currentNode.fCost && openList[i].gCost < currentNode.gCost)
                {
                    //set new current node
                    currentNode = openList[i];
                }
            }

            //Remove current node from open list & add to closed list
            openList.Remove(currentNode);
            closedList.Add(currentNode);

            //If at the target node - end the loop
            if (currentNode == targetNode)
            {
                RetracePath(startNode, targetNode);
                return;
            }
            else
            {
                //Otherwise continue finding path to target using neighbours - loop through each neighbour
                foreach (Node neighbour in WorldScanner.instance.GetNeighbours(currentNode))
                {
                    if (neighbour.isBlocked || closedList.Contains(neighbour))
                    {
                        //ignore this neighbour
                        continue;
                    }
                    else
                    {
                        //Calculate hCost
                        neighbour.hCost = DistanceCheck(neighbour, targetNode);
                        currentNode.hCost = DistanceCheck(currentNode, targetNode);

                        //check if neighbours distance to target is less then my current distance OR if neighbour is not in openlist
                        if (neighbour.hCost < currentNode.hCost || !openList.Contains(neighbour))
                        {
                            //Using world positions to calculate gCost
                            Vector2 neighbourPos = WorldScanner.instance.NodeToWorldPos(neighbour);
                            Vector2 currentNodePos = WorldScanner.instance.NodeToWorldPos(currentNode);

                            //Calculate neighbour gCost - if neighbour is diagonal gCost is double
                            if (neighbourPos.y == currentNodePos.y || neighbourPos.y != currentNodePos.y && neighbourPos.x == currentNodePos.x)
                            {
                                neighbour.gCost = 7;
                            }
                            else
                            {
                                neighbour.gCost = 14;
                            }

                            //set parent to keep track of our path
                            neighbour.parent = currentNode;

                            if (!openList.Contains(neighbour))
                            {
                                openList.Add(neighbour);
                            }
                        }
                    }
                }
            }
        }
    }

    //Retrace the finalized path
    Vector3 RetracePath(Node start, Node end)
    {
        Node currentNode = end;

        while (currentNode != start)
        {
            //Create a path from the end node to the start node following the node parent trail
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }

        path.Reverse();

        WorldScanner.instance.path = path;
        WorldScanner.instance.endNode = end;

        //Astar being an instance is setting a target pos for every entity that subscribes to this
        //pathFoundEvent?.Invoke(path);
        //entityUsingAstar.GetComponent<PathTracker>().GeneratePathList(path);

        return Vector3.zero;
    }

    int DistanceCheck(Node a, Node b)
    {
        return (int)Vector2.Distance(WorldScanner.instance.NodeToWorldPos(a), WorldScanner.instance.NodeToWorldPos(b));
    }
}

public class PathfindingThreadTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // Complete an arbitrary number of jobs
        NativeArray<JobHandle> handles = new NativeArray<JobHandle>(100, Allocator.Temp);

        for (int i = 0; i < 10; i++)
        {
            JohnPathfindJob johnPathfindJob = new JohnPathfindJob();
            JobHandle jobHandle1 = johnPathfindJob.Schedule();

            handles[i] = jobHandle1;
        }

        JobHandle.CompleteAll(handles);

        handles.Dispose();

    }
}
