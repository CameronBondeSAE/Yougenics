using System;
using System.Collections;
using System.Collections.Generic;
using Minh;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Minh
{
    public class Terrain2DGeneator : MonoBehaviour
    {
        public float Terrain2D;
        public float extended;
        public int blockNo = 20;
        public GameObject terrainblock;
        public int adjustNo;
       
        void Start()
        {
            for (int i = 0; i < blockNo; i++)
            {
                Terrain2D = Random.Range(0, blockNo);
                Instantiate(terrainblock, new Vector3(1, 0, 0), Quaternion.identity);




            }

;
        }

            // Update is called once per frame
        void Update()
        {
                
        }
        
        // Start is called before the first frame update

    }
}

