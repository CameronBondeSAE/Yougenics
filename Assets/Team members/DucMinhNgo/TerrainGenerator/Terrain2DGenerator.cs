using System;
using System.Collections;
using System.Collections.Generic;
using Minh;
using Tanks;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Minh
{
    public class Terrain2DGenerator : MonoBehaviour
    {
        public float terrain2D = 10f;
        public float extended = 2f;
        public int blockNo = 2;
        public GameObject cubeprefab;
        public int adjustNo = 10;
        public float noisevalue3D = 0f;
       
        void Start()
        {
            
            for (int x = 1; x < blockNo; x++)
            {
                for (int y = 1; y < blockNo; y++)
                {
                    terrain2D = Mathf.PerlinNoise(x * extended , y * extended);
                    GameObject instantiatedGO = Instantiate(cubeprefab);
                    instantiatedGO.transform.position = new Vector3(x, terrain2D * adjustNo, y );
                    

                }
            }
        }
        

       

        

        // Update is called once per frame
        
        
        // Start is called before the first frame update

    }
}

