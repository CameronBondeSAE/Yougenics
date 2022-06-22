using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.Mathematics;
using UnityEngine;

namespace Ollie
{
    public class WaterNode
    {
        public bool isBlocked;
        public bool isTooSteep;
        public Vector2 gridPosition;
        public float fillAmount;

        public WaterNode[,] neighbours = new WaterNode[3, 3];

        public void FillNeighbours()
        {
            for (int x = 0; x < 3; x++)
            {
                for (int z = 0; z < 3; z++)
                {
                    WaterNode neighbour = neighbours[x, z];

                    //need to ask Luke about this bit
                    if (!(neighbour == null || neighbour.isBlocked))
                    {
                        neighbour.fillAmount += 0.1f;
                        if (neighbour.fillAmount >= 1)
                        {
                            neighbour.FillNeighbours();
                        }
                    }
                }
            }
        }
    }
}