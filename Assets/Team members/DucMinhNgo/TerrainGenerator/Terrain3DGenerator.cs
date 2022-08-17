using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Terrain3DGenerator : MonoBehaviour
{
    public float terrain3D = 10f;
    public float extended = 0.2f;
    public float adjustNo = 0.1f;
    public TerrainGenerator terrainGenerator;

    void Start()
    {
        //GetComponent<TerrainGenerator>().Generatorformin += TerrainGen;
        terrainGenerator.calculateHeightCallback = Heightterrain;
        terrainGenerator.GenerateTerrain();
    }

    float Heightterrain(int x, int z)
    {

        terrain3D = Mathf.PerlinNoise(x * extended, z * extended);

        return terrain3D * adjustNo;

    }
}



