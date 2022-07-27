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
        public float fillAmount;
        public LevelManager levelManager;
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
                return new Vector3(gridPosition.x+levelManager.lengthX,1,gridPosition.y+levelManager.lengthZ);
            }
        }
        public WaterNode parent;

        public WaterNode[,] neighbours = new WaterNode[3, 3];

        public void ScanMyself()
        {
            var vector3 = new Vector3(levelManager.lengthX+gridPosition.x, 0, levelManager.lengthZ+gridPosition.y);
            if (Physics.OverlapBox(vector3, levelManager.gridTileHalfExtents, Quaternion.identity, levelManager.layers)
                    .Length != 0)
            {
                isBlocked = true;
                if(!levelManager.blockedNodes.Contains(this)) levelManager.blockedNodes.Add(this);
            }

            if (Physics.OverlapBox(vector3, levelManager.gridTileHalfExtents, Quaternion.identity, levelManager.layers)
                    .Length == 0)
            {
                isBlocked = false;
                if(levelManager.blockedNodes.Contains(this)) levelManager.blockedNodes.Remove(this);
            }
        }
        
        public void CheckNeighbours()
        {
            if (!levelManager.AStar)
            {
                for (int x = 0; x < 3; x++)
                {
                    for (int z = 0; z < 3; z++)
                    {
                        WaterNode neighbour = neighbours[x, z];
                    
                        if (!(neighbour == null || neighbour.isBlocked || neighbour.isWater))
                        {
                            if (!levelManager.openNodesToAdd.Contains(neighbour))
                            {
                                levelManager.openNodesToAdd.Add(neighbour);
                            }
                        }
                    }
                }
            }

            if (levelManager.AStar)
            {
                for (int x = 0; x < 3; x++)
                {
                    for (int z = 0; z < 3; z++)
                    {
                        WaterNode neighbour = neighbours[x, z];
                    
                        if (!(neighbour == null || neighbour.isBlocked || neighbour.isOpen))
                        {
                            if (!levelManager.openNodesToAdd.Contains(neighbour))
                            {
                                levelManager.openNodesToAdd.Add(neighbour);
                            }
                        }
                    }
                }
            }
        }
        
        public void FillNeighbours()
        {
            /*for (int x = 0; x < 3; x++)
            {
                for (int z = 0; z < 3; z++)
                {
                    WaterNode neighbour = neighbours[x, z];
                    
                    if (!(neighbour == null || neighbour.isBlocked || neighbour.isWater))
                    {
                        levelManager.SpreadToNeighbours(neighbour, neighbour.gridPosition);
                        neighbours[x, z].isWater = true;
                    }
                }
            }*/

            // foreach (WaterNode neighbour in levelManager.openWaterNodes)
            // {
            //     neighbour.isWater = true;
            //     levelManager.closedWaterNodes.Add(neighbour);
            //     levelManager.openWaterNodes.Remove(neighbour);
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