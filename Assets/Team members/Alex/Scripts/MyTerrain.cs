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
        public GameObject energyBall;

        public void Start()
        {
            // Set the callback to a temp randomiser function
            terrainGenerator.calculateHeightCallback = YourHeightCalculatorFunction;
            terrainGenerator.GenerateTerrain();
        }

        public float YourHeightCalculatorFunction(int x, int z)
        {
            perlinNoise = Mathf.PerlinNoise(x * scaler, z * scaler);

            GameObject energyBalls = energyBall;
            if (energyBall != null)
                if (Random.Range(0, 100) == 0 && (perlinNoise * heightMultiplier) > .6f)
                {
                    Instantiate(energyBall, new Vector3(z + transform.position.x, perlinNoise * heightMultiplier * terrainGenerator.depth + transform.position.y + 2f,x + transform.position.z), Quaternion.identity);
                    
                }

            return perlinNoise * heightMultiplier;
            
        }
        

    }
    
    
}

