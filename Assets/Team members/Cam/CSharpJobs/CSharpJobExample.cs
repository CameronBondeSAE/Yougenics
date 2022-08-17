using System.Collections;
using System.Collections.Generic;
using Unity.Jobs;
using UnityEngine;

public struct CamJob : IJob
{
    public float thing;
    public float stuff;
    
    public void Execute()
    {
        float answer = 0;

        for (int i = 0; i < 10000000; i++)
        {
            answer += Mathf.Sqrt(i) + Mathf.PerlinNoise(i * 1.24f, 0);
        }

        Debug.Log("I did something! : " + answer + " : Thing = "+thing);
    }
}
