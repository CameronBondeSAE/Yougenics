using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kevin
{
    public class Spawner : MonoBehaviour
    {
        public GameObject critterPrefab;
        public int spawnAmount;
        void Awake()
        {
            for (int i = 0; i < spawnAmount; i++)
            {
                float randomX = Random.Range(-60f, 60f);
                float randomZ = Random.Range(-60f, 60f);
                Instantiate(critterPrefab, new Vector3(randomX, 0f, randomZ),Quaternion.identity);
            }
        }
    }

}
