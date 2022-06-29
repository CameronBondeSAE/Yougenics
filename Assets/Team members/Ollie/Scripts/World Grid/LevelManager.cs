using System;
using System.Collections;
using System.Collections.Generic;
using Luke;
using Ollie;
using Unity.Mathematics;
using UnityEngine;
using Random = System.Random;

namespace Ollie
{
    public class LevelManager : MonoBehaviour
    {
        public Bounds bounds  = new Bounds(Vector3.zero, new Vector3(80,0,80));
        public WaterNode[,] gridNodeReferences;
        public Vector3 gridTileHalfExtents = new(0.5f, 0.5f, 0.5f);
        public int sizeX;
        public int sizeZ;
        public int lengthX;
        public int lengthZ;
        public Vector2 currentPosition;
        public LayerMask layers;
        public GameObject waterCube;
        
        public List<WaterNode> blockedNodes;
        //public List<WaterNode> allNodes;

        public List<Transform> testList;
        public List<WaterNode> openNodeV3List;
        public List<WaterNode> activeWaterNodes;
        public List<Vector3> waterPosList;
        public List<WaterNode> temporaryOpen;
        public List<WaterNode> openWaterNodes;
        public List<WaterNode> closedWaterNodes;

        void Start()
        {
            sizeX = Mathf.RoundToInt(bounds.extents.x) + 1;
            sizeZ = Mathf.RoundToInt(bounds.extents.z) + 1;
            lengthX = Mathf.RoundToInt(bounds.center.x - bounds.extents.x/2);
            lengthZ = Mathf.RoundToInt(bounds.center.z - bounds.extents.z/2);
            gridNodeReferences = new WaterNode[sizeX, sizeZ];

            //ScanWorld();
            blockedNodes = new List<WaterNode>();
            openNodeV3List = new List<WaterNode>();
            activeWaterNodes = new List<WaterNode>();
            temporaryOpen = new List<WaterNode>();
            waterPosList = new List<Vector3>();
            openWaterNodes = new List<WaterNode>();
            closedWaterNodes = new List<WaterNode>();
        }

        public void ScanWorld()
        {
            for (int x = 0; x < sizeX; x++)
            {
                for (int z = 0; z < sizeZ; z++)
                {
                    gridNodeReferences[x, z] = new WaterNode();
                    gridNodeReferences[x, z].gridPosition = new Vector2Int(x, z);
                    gridNodeReferences[x, z].levelManager = this;
                    
                    //allNodes.Add(gridNodeReferences[x,z]);
                    var vector3 = new Vector3(lengthX+x, 0, lengthZ+z);
                    //((-sizeX/2) + x)
                    //Debug.DrawRay(vector3,Vector3.up * 100, Color.magenta,10f);
                    if (Physics.OverlapBox(vector3, gridTileHalfExtents,
                        Quaternion.identity,layers).Length != 0)
                    {
                        //something is there
                        gridNodeReferences[x, z].isBlocked = true;
                        
                        //need to ask Cam why this fucks the whole grid?
                        //should just be adding their transforms to a list?
                        //but breaks the 3/4 of the whole grid
                        blockedNodes.Add(gridNodeReferences[x,z]);
                        //allNodes.Remove(gridNodeReferences[x, z]);
                    }
                }
            }

            //AssignNeighbours();
        }

        public void AssignNeighbours()
        {
            for (int x = 0; x < sizeX; x++)
            {
                for (int z = 0; z < sizeZ; z++)
                {
                    /*//if not top left, set top left neighbour
                    if(x > 0 && z > 0) gridNodeReferences[x, z].neighbours[0,0] = gridNodeReferences[x-1,z-1];
                    
                    //if not top row, set top middle neighbour
                    if(x > 0) gridNodeReferences[x, z].neighbours[0,1] = gridNodeReferences[x-1,z];
                    
                    //if not top right, set top right neighbour
                    if(x < sizeX-2 && z > 0) gridNodeReferences[x, z].neighbours[0,2] = gridNodeReferences[x-1,z+1];
                    
                    //if not left side, set left middle neighbour
                    if(z > 0) gridNodeReferences[x, z].neighbours[1,0] = gridNodeReferences[x,z-1];
                    
                    //if not right side, set right middle neighbour
                    if(z < sizeZ - 2) gridNodeReferences[x, z].neighbours[1,2] = gridNodeReferences[x,z+1];
                    
                    //if not bottom left, set bottom left neighbour
                    if(x > 0 && z < sizeZ - 2) gridNodeReferences[x, z].neighbours[2,0] = gridNodeReferences[x+1,z-1];
                    
                    //if not bottom row, set bottom middle neighbour
                    if(z < sizeZ - 2) gridNodeReferences[x, z].neighbours[2,1] = gridNodeReferences[x+1,z];
                    
                    //if not bottom right, set bottom right neighbour
                    if(x < sizeX - 2 && z < sizeZ - 2) gridNodeReferences[x, z].neighbours[2,2] = gridNodeReferences[x+1,z+1];*/
                    
                    //can't figure out why mine isn't working above
                    //copied luke's below to just make some progress
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

        public void CheckNeighbours()
        {
            temporaryOpen = openWaterNodes;
            foreach (WaterNode waterNode in temporaryOpen)
            {
                waterNode.CheckNeighbours();
            }
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
            foreach (WaterNode neighbour in openWaterNodes)
            {
                neighbour.isWater = true;
                if(!closedWaterNodes.Contains(neighbour)) closedWaterNodes.Add(neighbour);
                if(openWaterNodes.Contains(neighbour))    openWaterNodes.Remove(neighbour);
            }
            
        }

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
        private void OnDrawGizmos()
        {
            if (!Application.isPlaying)
            {
                Gizmos.color = new Color(0.5f, 0.5f, 0.5f, 0.5f);
                Gizmos.DrawCube(bounds.center,bounds.extents);
            }
            
            
            for (int x = 0; x < sizeX; x++)
            {
                for (int z = 0; z < sizeZ; z++)
                {
                    if (gridNodeReferences[x,z] != null && gridNodeReferences[x, z].isBlocked)
                    {
                        Gizmos.color = Color.red;
                        Gizmos.DrawCube(new Vector3(lengthX+x,0,lengthZ+z),Vector3.one);
                    }
                    
                    if (gridNodeReferences[x,z] != null && !gridNodeReferences[x, z].isBlocked && !gridNodeReferences[x,z].isWater)
                    {
                        Gizmos.color = Color.green;
                        Gizmos.DrawCube(new Vector3(lengthX+x,0,lengthZ+z),Vector3.one);
                    }

                    if (gridNodeReferences[x, z] != null && gridNodeReferences[x, z].isWater)
                    {
                        Gizmos.color = Color.blue;
                    }
                }
            }
        }
    }
}