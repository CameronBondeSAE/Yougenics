using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Perlin : MonoBehaviour
{
    public GameObject cubeToShake;
    public GameObject cubeToWibble;
    public GameObject cubeToSin;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        cubeToShake.transform.position = new Vector3(2, Random.Range(0,1f), 0);
        cubeToWibble.transform.position = new Vector3(0, Mathf.PerlinNoise(Time.time,0) * 4, 0);
        cubeToSin.transform.position = new Vector3(4, MathF.Sin(Time.time) + Mathf.Sin(Time.time*2.36f) + Mathf.Sin(Time.time*5.36f) + Mathf.Sin(Time.time*8.36f), 0);
    }
}
