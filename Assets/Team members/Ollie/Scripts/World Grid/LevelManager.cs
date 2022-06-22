using System;
using System.Collections;
using System.Collections.Generic;
using Ollie;
using Unity.Mathematics;
using UnityEngine;

namespace Ollie
{
    public class LevelManager : MonoBehaviour
    {
        public Bounds bounds  = new Bounds(Vector3.zero, new Vector3(80,0,80));
        public WaterNode[,] gridNodeReferences;
        public Vector3 gridTileHalfExtents = new(0.5f, 0.5f, 0.5f);
        public int sizeX;
        public int sizeZ;
        public Vector2 currentPosition;
        public LayerMask ignore;
        [SerializeField]
        private List<WaterNode> blockedNodes;
        
        void Start()
        {
            gridNodeReferences = new WaterNode[sizeX, sizeZ];
            //ScanWorld();
            blockedNodes = new List<WaterNode>();
        }

        public void ScanWorld()
        {
            for (int x = 0; x < sizeX; x++)
            {
                for (int z = 0; z < sizeZ; z++)
                {
                    gridNodeReferences[x, z] = new WaterNode();
                    gridNodeReferences[x, z].gridPosition = new Vector2(x, z);
                    
                    Vector3 myPos = transform.position;
                    var vector3 = new Vector3(myPos.x+x, 0, myPos.z+z);
                    //((-sizeX/2) + x)
                    //Debug.DrawRay(vector3,Vector3.up * 100, Color.magenta,10f);
                    if (Physics.OverlapBox(vector3, gridTileHalfExtents,
                        Quaternion.identity,ignore).Length != 0)
                    {
                        //something is there
                        gridNodeReferences[x, z].isBlocked = true;
                        
                        //need to ask Cam why this fucks the whole grid?
                        //should just be adding their transforms to a list?
                        //but breaks the 3/4 of the whole grid
                        //blockedNodes.Add(gridNodeReferences[x,z]);
                    }
                }
            }
        }

        //commented out so I could push without errors popping up for others
        private void OnDrawGizmos()
        {
            /*Gizmos.color = new Color(0.5f, 0.5f, 0.5f, 0.5f);
            Gizmos.DrawCube(bounds.center,bounds.extents);
            
            for (int x = 0; x < sizeX; x++)
            {
                for (int z = 0; z < sizeZ; z++)
                {
                    Vector3 myPos = transform.position;
                    if (gridNodeReferences[x, z].isBlocked)
                    {
                        Gizmos.color = Color.red;
                        Gizmos.DrawCube(new Vector3(myPos.x+x,0,myPos.z+z),Vector3.one);
                    }

                    if (!gridNodeReferences[x, z].isBlocked)
                    {
                        Gizmos.color = Color.green;
                        Gizmos.DrawCube(new Vector3(myPos.x+x,0,myPos.z+z),Vector3.one);
                    }
                }
            }*/
        }
    }
}