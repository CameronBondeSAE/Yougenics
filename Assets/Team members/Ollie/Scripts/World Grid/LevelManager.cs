using System;
using System.Collections;
using System.Collections.Generic;
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
        public List<Vector3> waterPosList;
        
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
            waterPosList = new List<Vector3>();
        }

        public void ScanWorld()
        {
            for (int x = 0; x < sizeX; x++)
            {
                for (int z = 0; z < sizeZ; z++)
                {
                    gridNodeReferences[x, z] = new WaterNode();
                    gridNodeReferences[x, z].gridPosition = new Vector2Int(x, z);
                    
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
        }

        public void SpawnWater()
        {
            GameObject go = Instantiate(waterCube);
            
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

            Vector2Int transformPosition = openNodeV3List[UnityEngine.Random.Range(0,openNodeV3List.Capacity-1)].gridPosition;
            go.transform.position = new Vector3(transformPosition.x+lengthX,1,transformPosition.y+lengthZ);
            waterPosList.Add(go.transform.position);
        }

        public void SpreadToNeighbours()
        {
            for (int x = -1; x < 2; x++)
            {
                for (int z = -1; z < 2; z++)
                {
                    
                }
            }
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
                    
                    if (gridNodeReferences[x,z] != null && !gridNodeReferences[x, z].isBlocked)
                    {
                        Gizmos.color = Color.green;
                        Gizmos.DrawCube(new Vector3(lengthX+x,0,lengthZ+z),Vector3.one);
                    }
                }
            }
        }
    }
}