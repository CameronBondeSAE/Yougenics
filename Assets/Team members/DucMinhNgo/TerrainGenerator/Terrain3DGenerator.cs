using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Terrain3DGenerator : MonoBehaviour
{
    public float terrain3D = 10f;
    public float extended = 0.2f;
    public int blockNo = 2;
    public GameObject cubeprefab;
    public float adjustNo = 0.1f;
    public float noisevalue3D = 0.1f;
    public TerrainGenerator terrainGenerator;
    void Start()
    {
        GetComponent<TerrainGenerator>().Generatorformin += TerrainGen;
        terrainGenerator.calculateHeightCallback = Heightcal;
        terrainGenerator.GenerateTerrain();
        TerrainGen();
    }

    void TerrainGen()
    {
        for (int x = 1; x < blockNo; x++)
        {
            for (int y = 1; y < blockNo; y++)
            {
                for (int z = 1; y < blockNo; y++)
                {
                    float noiseValue = (float)NoiseS3D.Noise(50, 50);
                    terrain3D = Mathf.PerlinNoise(x * extended , y * extended);
                    
                    if (noiseValue >= 0f)
                    {
                        Instantiate(gameObject, new Vector3(x, y, z), Quaternion.identity);
                       
                    }
                    
                    
                }
            }
            
        }
    }
    float Heightcal(int x, int z)
    {
        
        terrain3D = Mathf.PerlinNoise(x * extended, z * extended);

        return terrain3D * adjustNo;

    }
    
}
