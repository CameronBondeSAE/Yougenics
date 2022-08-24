using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpawnerTest : MonoBehaviour
{
    public GameObject critterAPrefab;
    public GameObject critterBPrefab;
    public GameObject foodPrefab;
    public int spawnAmount;
    private float randomX;
    private float randomZ;
    [SerializeField] private Vector3 fieldSize = new();
    public bool critterA;
    public bool critterB;
    public bool spawnFood;
    public void Awake()
    {

    }

    public void Start()
    {
        if (critterA)
        {
            for (int x = 0; x < spawnAmount; x++)
            {
                randomX = Random.Range(-30f, 30f);
                randomZ = Random.Range(-30f, 30f);
                Vector3 spawnPoint = new Vector3(randomX, 1f, randomZ);
                Instantiate(critterAPrefab, spawnPoint, Quaternion.identity);
            }
        }

        if (critterB)
        {
            for (int x = 0; x < spawnAmount; x++)
            {
                randomX = Random.Range(-30f, 30f);
                randomZ = Random.Range(-30f, 30f);
                Vector3 spawnPoint = new Vector3(randomX, 1f, randomZ);
                Instantiate(critterBPrefab, spawnPoint, Quaternion.identity);
            }
        }

        if (spawnFood)
        {
            for (int x = 0; x < spawnAmount; x++)
            {
                randomX = Random.Range(-40f, 40f);
                randomZ = Random.Range(-40f, 40f);
                Vector3 spawnPoint = new Vector3(randomX, 1f, randomZ);
                Instantiate(foodPrefab, spawnPoint, Quaternion.identity);  
            }
         
        }
       
    }
    public void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, fieldSize);
    }
}