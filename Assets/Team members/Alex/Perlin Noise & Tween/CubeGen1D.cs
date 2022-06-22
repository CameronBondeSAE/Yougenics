using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeGen1D : MonoBehaviour
{
    public float perlinNoise = 0f;
    public int cubes = 0;
    public float scaler = .1f;
    public float multiplier = 10f;
    public GameObject cubePrefab;

    // Update is called once per frame
    void Start()
    {
        GenerateMyCube();
        //Instantiate(cubePrefab, new Vector3(0, 0, 0), Quaternion.identity);
    }
    
    private void GenerateMyCube()
    {
        for (int x = 1; x < cubes; x++)
        {
            perlinNoise = Mathf.PerlinNoise(x * scaler, 0);
            Instantiate(cubePrefab, new Vector3(x, perlinNoise * multiplier, 0), Quaternion.identity);
            Debug.Log(Mathf.PerlinNoise(x, 0));
        }
    }
   
}
