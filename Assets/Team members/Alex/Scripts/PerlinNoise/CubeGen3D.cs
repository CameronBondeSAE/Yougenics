using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class CubeGen3D : MonoBehaviour
{
    //public float perlinNoise = 0f;
    public float scaler = 0.08f;
    //public float multiplier = 0f;
    public int cubes = 20;
    public GameObject cubePrefab; 
    
   public bool randomizeNoiseOffSet;

    public Vector3 perlinOffset;

    void Start()
    {
        if (randomizeNoiseOffSet)
        {
            perlinOffset = new Vector3(Random.Range(1, 999999), Random.Range(1, 999999),Random.Range(1, 999999));
        }
        
        for (int x = 0; x < cubes; x++)
        {
            for (int z = 0; z < cubes; z++)
            {
                for (int y = 0; y < cubes; y++)
                {
                    float noiseValue = (float)NoiseS3D.NoiseCombinedOctaves(x * scaler + perlinOffset.x, y * scaler + perlinOffset.y, z * scaler + perlinOffset.z);

                    noiseValue = (noiseValue + 1) * 0.5f;
                    //Instantiate(cubePrefab, new Vector3(x * multiplier + noiseValue, y * multiplier + noiseValue, z * multiplier + noiseValue), Quaternion.identity);
                    if (noiseValue >= 0.5f)
                    {
                        Instantiate(cubePrefab, new Vector3(x, y, z), Quaternion.identity);
                    }
                }
            }
        }
    }

    
}
