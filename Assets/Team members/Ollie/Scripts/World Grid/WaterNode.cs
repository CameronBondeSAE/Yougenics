using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using NodeCanvas.Tasks.Actions;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

namespace Ollie
{
    public class WaterNode
    {
        public bool isBlocked;
        public bool isTooSteep;
        public Vector2Int gridPosition;
        public float fillAmount;
        public LevelManager levelManager;
        public bool isWater;

        public bool isOpen;
        public bool isClosed;
        public bool isPath;

        public int gCost;
        public int hCost;

        public int fCost
        {
            get
            {
                return gCost + hCost;
            }
        }
        public WaterNode parent;

        public WaterNode[,] neighbours = new WaterNode[3, 3];

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
    }
}