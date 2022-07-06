using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Terrain3DGenerator : MonoBehaviour
{
    public float terrain3D = 10f;
    public float extended = 2f;
    public int blockNo = 2;
    public GameObject cubeprefab;
    public int adjustNo = 10;
    public float noisevalue3D = 0f;
    public TerrainGenerator terrainGenerator;
    void Start()
    {
        terrainGenerator.calculateHeightCallback = YourHeightCalculatorFunction;
        terrainGenerator.GenerateTerrain();
        for (int x = 1; x < blockNo; x++)
        {
            for (int y = 1; y < blockNo; y++)
            {
                for (int z = 1; y < blockNo; y++)
                { 
                    float noiseValue = (float)NoiseS3D.Noise(50, 50);
                    noiseValue = (noiseValue + 1) * 0.5f;
                    terrain3D = Mathf.PerlinNoise(x * extended , y * extended);
                    GameObject instantiatedGO = Instantiate(cubeprefab);
                    if (noiseValue >= 0.5f)
                    {
                        Instantiate(cubeprefab, new Vector3(x, y, z), Quaternion.identity);
                        instantiatedGO.transform.position = new Vector3(x, terrain3D * adjustNo, y );
                    }
                    
                    
                }
            }
            
        }
        float YourHeightCalculatorFunction(int x, int z)
        {
        
            terrain3D = Mathf.PerlinNoise(x * extended, z * extended);

            return terrain3D * adjustNo;

        }
    }
}
