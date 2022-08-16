using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpawnerTest : MonoBehaviour
{
    public GameObject critterPrefab;
    public int spawnAmount;
    private float randomX;
    private float randomZ;
    [SerializeField] private Vector3 fieldSize = new();
    public void Awake()
    {

    }

    public void Start()
    {
        for (int x = 0; x < spawnAmount; x++)
        {
            randomX = Random.Range(-30f, 30f);
            randomZ = Random.Range(-30f, 30f);
            Vector3 spawnPoint = new Vector3(randomX, 1f, randomZ);
            Instantiate(critterPrefab, spawnPoint, Quaternion.identity);
        }
    }
    public void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, fieldSize);
    }
}