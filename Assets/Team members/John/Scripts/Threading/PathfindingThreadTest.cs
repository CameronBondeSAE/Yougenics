using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Jobs;
using Unity.Collections;
using System.Linq;

public struct JohnPathfindJob : IJob
{
    public Vector3 targetPos;
    public Vector3 startPos;
    public NativeArray<Vector2> pathList;

    public void Execute()
    {
        FindPath(startPos, targetPos);
    }

    public void FindPath(Vector3 _startPos, Vector3 _targetPos)
    {
        //Keep for ReScan Reference
        targetPos = _targetPos;
        startPos = _startPos;

        Node startNode = WorldScanner.instance.WorldToNodePos(_startPos);
        Node targetNode = WorldScanner.instance.WorldToNodePos(_targetPos);

        List<Node> openList = new List<Node>();
        List<Node> closedList = new List<Node>();

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
    void RetracePath(Node start, Node end)
    {
        //List<Vector2> path = new List<Vector2>();
        Node currentNode = end;

        int i = 0;

        while (currentNode != start && i < 100)
        {
            //Create a path from the end node to the start node following the node parent trail
            //path.Add(currentNode.gridPos);
            pathList[i] = currentNode.gridPos;
            currentNode = currentNode.parent;
            i++;
        }

        //path.Reverse();
        //pathList.Reverse();
        Debug.Log("Path Found");
        //
        //pathList = path.ToNativeArray<Vector2>(Allocator.TempJob);
    }

    int DistanceCheck(Node a, Node b)
    {
        return (int)Vector2.Distance(WorldScanner.instance.NodeToWorldPos(a), WorldScanner.instance.NodeToWorldPos(b));
    }
}

public class PathfindingThreadTest : MonoBehaviour
{
    Vector3 targetPos;
    Vector3 startPos;
    List<Vector2> path = new List<Vector2>();

    public bool useJobs;
    //public int jobCount = 1;

    public JohnPathTracker testAI;
    public JohnPathTracker[] testAIArray;
    //public List<GameObject> 

    // Start is called before the first frame update
    void Start()
    {
        if(useJobs)
        {
            // Complete an arbitrary number of jobs
            NativeArray<JobHandle> jobHandleArray = new NativeArray<JobHandle>(100, Allocator.Temp);

            //NativeArray<Vector2> myPathList = new NativeArray<Vector2>(100, Allocator.TempJob);

            //List of NativeArray's for each job to use
            List<NativeArray<Vector2>> pathListArray = new List<NativeArray<Vector2>>(testAIArray.Length);

            for (int i = 0; i < testAIArray.Length; i++)
            {
                //Add a new entry to the list
                pathListArray.Add(new NativeArray<Vector2>(100, Allocator.TempJob));

                JohnPathfindJob johnPathfindJob = new JohnPathfindJob
                {
                    startPos = testAIArray[i].transform.position,
                    targetPos = new Vector3(Random.Range(2, 50), Random.Range(2, 50), Random.Range(2, 50)),
                    pathList = pathListArray[i]
                };

                JobHandle jobHandle1 = johnPathfindJob.Schedule();

                jobHandleArray[i] = jobHandle1;
            }

            JobHandle.CompleteAll(jobHandleArray);

            //Give the path to the AI
            /*List<Vector2> finalPathList = pathListArray[0].ToList();
            finalPathList.Reverse();
            testAI.GeneratePathList(finalPathList);*/

            for (int i = 0; i < testAIArray.Length; i++)
            {
                List<Vector2> finalPathList = pathListArray[i].ToList();
                finalPathList.Reverse();
                testAIArray[i].GeneratePathList(finalPathList);
            }

            //Dispose each native array in the array of native arrays
            foreach (var pathList in pathListArray)
            {
                /*foreach (var pos in pathList)
                {
                    Debug.Log(pos);
                }*/

                pathList.Dispose();
            }

            //then dispose the whole array
            //pathListArray.Dispose();

            jobHandleArray.Dispose();
        }
        else
        {
            for (int i = 0; i < testAIArray.Length; i++)
            {
                FindPath(testAIArray[i].transform.position, new Vector3(Random.Range(2, 50), Random.Range(2, 50), Random.Range(2, 50)), testAIArray[i]);
            }
        }
    }

    public void FindPathUsingJob(Vector3 startPos)
    {
        //Variable to copy the data into
        NativeArray<Vector2> myPathList = new NativeArray<Vector2>(100, Allocator.TempJob);

        //Create the Job
        JohnPathfindJob johnPathfindJob = new JohnPathfindJob
        {
            startPos = testAI.transform.position,
            targetPos = new Vector3(Random.Range(2, 50), Random.Range(2, 50), Random.Range(2, 50)),
            pathList = myPathList
        };

        //Schedule the Job
        JobHandle jobHandle1 = johnPathfindJob.Schedule();

        jobHandle1.Complete();

        //Give the path to the AI
        List<Vector2> finalPathList = myPathList.ToList();
        finalPathList.Reverse();
        testAI.GeneratePathList(finalPathList);

        //Dispose
        myPathList.Dispose();

    }

    public void FindPath(Vector3 _startPos, Vector3 _targetPos, JohnPathTracker user)
    {
        //Keep for ReScan Reference
        targetPos = _targetPos;
        startPos = _startPos;

        Node startNode = WorldScanner.instance.WorldToNodePos(_startPos);
        Node targetNode = WorldScanner.instance.WorldToNodePos(_targetPos);

        List<Node> openList = new List<Node>();
        List<Node> closedList = new List<Node>();

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
                RetracePath(startNode, targetNode, user);
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
    void RetracePath(Node start, Node end, JohnPathTracker user)
    {
        Node currentNode = end;

        while (currentNode != start)
        {
            //Create a path from the end node to the start node following the node parent trail
            path.Add(currentNode.gridPos);
            currentNode = currentNode.parent;
        }

        path.Reverse();
        Debug.Log("Path Found");
        user.GeneratePathList(path);
    }

    int DistanceCheck(Node a, Node b)
    {
        return (int)Vector2.Distance(WorldScanner.instance.NodeToWorldPos(a), WorldScanner.instance.NodeToWorldPos(b));
    }
}
