using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using Random = UnityEngine.Random;
using DG.Tweening;
using Unity.VisualScripting;

namespace Alex
{
    public class MyTerrain : MonoBehaviour
    {
        public TerrainGenerator terrainGenerator;

        public float scalerOne = .1f;
        public float scalerTwo = .2f;
        public float scalerRange;
        public float scalerOld;
        public float scalerNew;
        private float perlinNoise = 0f;
        public float heightMultiplier = 10f;
        //public GameObject energyBall;
        //public bool terraforming;
        //public float changeRate = 1f;
        //public float terraformCount = 3;
        public List<GameObject> thingToSpawn = new List<GameObject>();
        Spawner spawner;

        public void Start()
        {
            spawner = GetComponent<Spawner>();
            //DayNightManager.instance.ChangePhase(DayNightManager.DayPhase.Midnight);
            scalerRange = Random.Range(scalerOne, scalerTwo);
            scalerOld = scalerRange;
            // Set the callback to a temp randomiser function
            terrainGenerator.calculateHeightCallback = YourHeightCalculatorFunction;
            terrainGenerator.GenerateTerrain();
            //DayNightManager.instance.PhaseChangeEvent += ChangeTerrain;
        }

    


        public void ChangeTerrain(DayNightManager.DayPhase phase)
        {
            if (phase != DayNightManager.DayPhase.Noon)
                return; 
            
            float targetScaler = Random.Range(scalerOne, scalerTwo);
            //terrainGenerator.calculateHeightCallback = YourHeightCalculatorFunction;
            //terrainGenerator.GenerateTerrain();
            StartCoroutine(Terraform(targetScaler));
        }

        public IEnumerator Terraform(float targetScaler)
        {
            scalerNew = Random.Range(scalerOne, scalerTwo);
            scalerRange = Mathf.Lerp(scalerOld, scalerNew, 0.05f);

            while (scalerRange != Mathf.Lerp(scalerOld, scalerNew, 0.05f))
            {
                terrainGenerator.GenerateTerrain();
                
                yield return new WaitForSeconds(0.5f);
                //scalerRange = Mathf.Lerp(scalerRange, targetScaler, 0.001f);
            }

            scalerRange = targetScaler;
            terrainGenerator.GenerateTerrain();

            spawner.shouldISpawnDelegate = ShouldISpawnDelegate;
            spawner.SpawnMultiple();

            /*
            int i = 0;
            
            scalerRange = Mathf.Lerp(scalerRange, targetScaler, 10f);
            do
            {
                terrainGenerator.GenerateTerrain();
                yield return new WaitForSeconds(changeRate);
                i++;
            }
            
            while (i < terraformCount);
            */

        }

        bool ShouldISpawnDelegate(float x, float y)
        {
            return true; // TODO replace
        }


        public float YourHeightCalculatorFunction(int x, int z)
        {
            perlinNoise = Mathf.PerlinNoise(x * scalerRange, z * scalerRange);
            
            if (thingToSpawn.Count > 0)
                if (Random.Range(0, 100) == 0 && perlinNoise > .5f)
                {
                    spawner.SpawnSingle(thingToSpawn[Random.Range(0,thingToSpawn.Count)], 
                                        new Vector3(x, perlinNoise * terrainGenerator.depth,z) + transform.position, Quaternion.identity);
                    //Instantiate(energyBall, new Vector3(z + transform.position.x, perlinNoise * heightMultiplier * terrainGenerator.depth + transform.position.y + 2f,x + transform.position.z), Quaternion.identity);
                }
            
            
            return perlinNoise * heightMultiplier;
        }
        

    }
    
    
}

