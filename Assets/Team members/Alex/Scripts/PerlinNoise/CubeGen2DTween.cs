using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CubeGen2DTween : MonoBehaviour
{
    public float perlinNoise = 0f;
    public float scaler = .1f;
    public float multiplier = 10f;
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
        for (int x = 0; x < cubes; x++)
        {
            for (int z = 0; z < cubes; z++)
            {
                perlinNoise = Mathf.PerlinNoise(x * scaler, z * scaler);
                GameObject instantiated = Instantiate(cubePrefab, new Vector3(x, 0, z), Quaternion.identity);
                instantiated.transform.DOMoveY(perlinNoise * multiplier,duration);
            }
        }     
        
        
        
        //cubePrefab.transform.localPosition = new Vector3(0, Mathf.PerlinNoise(Time.time, 0), 0);
        //transform.DOLocalMoveY(target,duration);
    }
}
