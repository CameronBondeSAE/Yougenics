using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using NodeCanvas.Tasks.Actions;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

namespace Ollie
{
    public class WaterNode : iHeapItem<WaterNode>
    {
        public bool isBlocked;
        public bool isTooSteep;
        public Vector2Int gridPosition;
        
        public int xPosInArray;
        public int zPosInArray;
        
        public float fillAmount;
        public bool isWater;
        public bool startLocation;
        public bool targetLocation;

        public bool isOpen;
        public bool isClosed;
        public bool isPath;

        private int _heapIndex;

        public int gCost;
        public int hCost;

        public int fCost
        {
            get
            {
                return gCost + hCost;
            }
        }

        public Vector3 gridPosVector3
        {
            get
            {
                //will need to update Y pos when heights are implemented
                return new Vector3(gridPosition.x,1,gridPosition.y);
            }
        }
        public WaterNode parent;

        public WaterNode[,] neighbours = new WaterNode[3, 3];

        public void ScanMyself()
        {
            var vector3 = new Vector3(gridPosition.x, 0, gridPosition.y);
            if (Physics.OverlapBox(vector3, LevelManager.instance.gridTileHalfExtents, Quaternion.identity, LevelManager.instance.layers)
                    .Length != 0)
            {
                isBlocked = true;
                if(!LevelManager.instance.blockedNodes.Contains(this)) LevelManager.instance.blockedNodes.Add(this);
            }

            if (Physics.OverlapBox(vector3, LevelManager.instance.gridTileHalfExtents, Quaternion.identity, LevelManager.instance.layers)
                    .Length == 0)
            {
                isBlocked = false;
                if(LevelManager.instance.blockedNodes.Contains(this)) LevelManager.instance.blockedNodes.Remove(this);
            }
        }
        
        /*public void CheckNeighbours()
        {
            if (!LevelManager.instance.AStar)
            {
                for (int x = 0; x < 3; x++)
                {
                    for (int z = 0; z < 3; z++)
                    {
                        WaterNode neighbour = neighbours[x, z];
                    
                        if (!(neighbour == null || neighbour.isBlocked || neighbour.isWater))
                        {
                            if (!LevelManager.instance.openNodesToAdd.Contains(neighbour))
                            {
                                LevelManager.instance.openNodesToAdd.Add(neighbour);
                            }
                        }
                    }
                }
            }

            if (LevelManager.instance.AStar)
            {
                for (int x = 0; x < 3; x++)
                {
                    for (int z = 0; z < 3; z++)
                    {
                        WaterNode neighbour = neighbours[x, z];
                    
                        if (!(neighbour == null || neighbour.isBlocked || neighbour.isOpen))
                        {
                            if (!LevelManager.instance.openNodesToAdd.Contains(neighbour))
                            {
                                LevelManager.instance.openNodesToAdd.Add(neighbour);
                            }
                        }
                    }
                }
            }
        }*/
        
        public void FillNeighbours()
        {
            /*for (int x = 0; x < 3; x++)
            {
                for (int z = 0; z < 3; z++)
                {
                    WaterNode neighbour = neighbours[x, z];
                    
                    if (!(neighbour == null || neighbour.isBlocked || neighbour.isWater))
                    {
                        LevelManager.instance.SpreadToNeighbours(neighbour, neighbour.gridPosition);
                        neighbours[x, z].isWater = true;
                    }
                }
            }*/

            // foreach (WaterNode neighbour in LevelManager.instance.openWaterNodes)
            // {
            //     neighbour.isWater = true;
            //     LevelManager.instance.closedWaterNodes.Add(neighbour);
            //     LevelManager.instance.openWaterNodes.Remove(neighbour);
            // }
        }
        
        public int CompareTo(WaterNode nodeToCompare) //returns 1 if nodeToCompare is HIGHER
        {
            //check this node's fCost against node to compare
            int compare = fCost.CompareTo(nodeToCompare.fCost);
            
            //0 if identical f cost, so check h cost
            if (compare == 0)
            {
                compare = hCost.CompareTo(nodeToCompare.hCost);
            }
            
            //we want to return 1 if higher priority, but this returns 1 if nodeToCompare is higher so need to reverse
            return -compare;
        }

        public int heapIndex
        {
            get
            {
                return _heapIndex;
            }
            set
            {
                _heapIndex = value;
            }
        }
    }
}