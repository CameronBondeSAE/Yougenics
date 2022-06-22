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
    public class Terrain2DGeneator : MonoBehaviour
    {
        public float terrain1D = 10f;
        public float extended = 2f;
        public int blockNo = 2;
        public GameObject cube;
        public GameObject cube2;
        public GameObject cube3;
        public int adjustNo = 10;
       
        void Start()
        {
            Generatecube2();
            Generatecube3();
            for (int i = 1; i < blockNo; i++)
            {
                terrain1D = Mathf.PerlinNoise(i * extended, Mathf.PerlinNoise(Time.time, 0));
                GameObject instantiatedGO = Instantiate(cube);
                instantiatedGO.transform.position = new Vector3(0, terrain1D * adjustNo, 0);
                cube.transform.position = new Vector3(0, Mathf.PerlinNoise(Time.time, 0), 0);
                cube2.transform.position = new Vector3(2, Random.Range(0,1f), 0);
                cube3.transform.position = new Vector3(4, MathF.Sin(Time.time) * Mathf.Sin(Time.time* 1.1f), 0);
                
            }
        }

        void Generatecube2()
        {
            GameObject instantiatedGO = Instantiate(cube2);
            instantiatedGO.transform.position = new Vector3(0, terrain1D * adjustNo, 0);
        }

        void Generatecube3()
        {
            GameObject instantiatedGO = Instantiate(cube3);
            instantiatedGO.transform.position = new Vector3(0, terrain1D * adjustNo, 0);
        }

        

        // Update is called once per frame
        
        
        // Start is called before the first frame update

    }
}

