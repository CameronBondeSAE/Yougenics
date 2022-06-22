using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CubeGen2D : MonoBehaviour
{
    public float perlinNoise = 0f;
    public float scaler = 0f;
    public float multiplier = 0f;
    public int cubes = 0;
    public GameObject cubePrefab; 
    
    public float target;
    public float duration;

    public bool randomizeNoiseOffSet;

    public Vector2 perlinOffset;
    //public List<ListOfCubes> ListOfCubesList; 
    
    // Start is called before the first frame update
    void Start()
    {
        if (randomizeNoiseOffSet)
        {
            perlinOffset = new Vector2(Random.Range(1, 99999), Random.Range(1, 9999));
        }
        
        for (int x = 0; x < cubes; x++)
        {
            for (int z = 0; z < cubes; z++)
            {
                perlinNoise = Mathf.PerlinNoise(x * scaler * perlinOffset.x, z * scaler * perlinOffset.y);
                Instantiate(cubePrefab, new Vector3(x, perlinNoise * multiplier, z), Quaternion.identity);
                cubePrefab.transform.DOMoveY(Mathf.PerlinNoise(Time.time, 0),duration);
            }
        }     
        
        
        //cubePrefab.transform.localPosition = new Vector3(0, Mathf.PerlinNoise(Time.time, 0), 0);
        //transform.DOLocalMoveY(target,duration);
    }
}
