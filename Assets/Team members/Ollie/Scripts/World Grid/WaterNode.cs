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

        public WaterNode[,] neighbours = new WaterNode[3, 3];

        public void CheckNeighbours()
        {
            for (int neighbourX = -1; neighbourX < 2; neighbourX++)
            {
                for (int neighbourY = -1; neighbourY < 2; neighbourY++)
                {
                    //neighbours[gridPosition.x + neighbourX, gridPosition.y + neighbourY];
                    
                }
            }
        }
        public void FillNeighbours()
        {
            for (int x = 0; x < 3; x++)
            {
                for (int z = 0; z < 3; z++)
                {
                    WaterNode neighbour = neighbours[x, z];
                    
                    if (!(neighbour == null || neighbour.isBlocked))
                    {
                        levelManager.SpreadToNeighbours(this);
                    }
                }
            }
        }
    }
}