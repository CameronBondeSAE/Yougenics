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
    public class Terrain1DGenerator : MonoBehaviour
    {
        public float terrain1D = 10f;
        public float extended = 2f;
        public int blockNo = 2;
        public GameObject cubeprefab;
        public int adjustNo = 10;
       
        void Start()
        {
            
            for (int x = 1; x < blockNo; x++)
            {
                
                    terrain1D = Mathf.PerlinNoise(x * extended, 0);
                    GameObject instantiatedGO = Instantiate(cubeprefab);
                    instantiatedGO.transform.position = new Vector3(x, terrain1D * adjustNo , 0 );
            }
        }
    }
}



