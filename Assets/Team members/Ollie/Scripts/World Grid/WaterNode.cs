using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.Mathematics;
using UnityEngine;

namespace Ollie
{
    public class WaterNode
    {
        public WaterNode[,] gridNodeReferences;
        public bool isBlocked;
        public Vector3 size;
        public Vector3 gridSize;
        
        // Start is called before the first frame update
        void Start()
        {
            ScanWorld();
        }

        // Update is called once per frame
        void Update()
        {

        }

        void ScanWorld()
        {
            for (int x = 0; x < size.x; x++)
            {
                for (int y = 0; y < size.y; y++)
                {
                    // if (Physics.OverlapBox(new Vector3(x * gridSize.x, 0, y * gridSize.z),
                    //     new Vector3(gridSize.x, gridSize.y, gridSize.z), quaternion.identity))
                    // {
                    //     //something is there
                    //     gridNodeReferences[x, y].isBlocked = true;
                    // }
                }
            }
        }
    }
}