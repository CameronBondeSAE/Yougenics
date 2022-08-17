using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Alex
{
    public class MyTerrain : MonoBehaviour
    {
        public TerrainGenerator terrainGenerator;

        public float scaler = .1f;
        private float perlinNoise = 0f;
        public float heightMultiplier = 10f;


        public void Start()
        {
            // Set the callback to a temp randomiser function
            terrainGenerator.calculateHeightCallback = YourHeightCalculatorFunction;
            terrainGenerator.GenerateTerrain();
        }

        public float YourHeightCalculatorFunction(int x, int z)
        {
            perlinNoise = Mathf.PerlinNoise(x * scaler, z * scaler);

            return perlinNoise * heightMultiplier;
        }
    }
    
    
}

