using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Maya
{
    public class LevelBuilder : MonoBehaviour
    {
        public Node[,] gridNodeReferences;
        public Vector3 halfGrids = new Vector3(0.5f, 0.5f, 0.5f);
        public int sizeX;
        public int sizeZ;

        void Start()
        {
            gridNodeReferences = new Node[sizeX, sizeZ];
            WorldScanner();
        }

        private void WorldScanner()
        {
            for (int x = 0; x < sizeX; x++)
            {
                for (int z = 0; z < sizeZ; z++)
                {
                    gridNodeReferences[x, z] = new Node();
                    Vector3 myPos = transform.position;
                    if (Physics.OverlapBox(new Vector3((-sizeX/2)+x, 0, (-sizeZ/2)+z), halfGrids, Quaternion.identity).Length != 0)
                    {
                        gridNodeReferences[x, z].isBlocked = true;
                    }
                }
            }
        }

        private void OnDrawGizmos()
        {
            if (Application.isPlaying)
            {
                for (int x = 0; x < sizeX; x++)
                {
                    for (int z = 0; z < sizeZ; z++)
                    {
                        if (gridNodeReferences[x, z].isBlocked)
                        {
                            Gizmos.color = Color.cyan;
                            Gizmos.DrawCube(new Vector3((-sizeX/2)+x, 0, (-sizeZ/2)+z), Vector3.one);
                        }
                        else
                        {
                            Gizmos.color = Color.yellow;
                            Gizmos.DrawCube(new Vector3((-sizeX/2)+x, 0, (-sizeZ/2)+z), Vector3.one);
                        }
                    }
                }
            }
        }
    }
}

    
    
