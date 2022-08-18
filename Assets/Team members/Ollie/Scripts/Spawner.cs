using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ollie
{
    public class Spawner : MonoBehaviour
    {
        public NpcType npcType;

        public GameObject food;
        public GameObject predator;
        public GameObject critter;

        public int spawnAmount;

        private Vector3 spawnLocation;
        private int planeFloor = 1;
        
        void Start()
        {
            CreateNPCs(spawnAmount);
        }
        
        public enum NpcType
        {
            Food,
            Predator,
            Critter
        }

        void CreateNPCs(int spawnAmount)
        {
            for (int i = 0; i < spawnAmount; i++)
            {
                //should update this to take in a spot from the gridnodereferences
                //otherwise, currently it can spawn on a blocked node (eg inside walls!)
                
                int posX = Random.Range((-LevelManager.instance.sizeX/2)+LevelManager.instance.offsetX,(LevelManager.instance.sizeX/2)+LevelManager.instance.offsetX);
                int posY = planeFloor;
                int posZ = Random.Range((-LevelManager.instance.sizeZ/2)+LevelManager.instance.offsetZ,(LevelManager.instance.sizeZ/2)+LevelManager.instance.offsetZ);
                
                spawnLocation = new Vector3(posX, posY, posZ);
                
                var rng = UnityEngine.Random.Range(1, 101);
                if (rng <= 100) //set to 40 if randomising all 3
                {
                    GameObject go = Instantiate(food);
                    go.transform.position = spawnLocation;
                }
                else if (rng >= 41 && rng <= 80)
                {
                    GameObject go = Instantiate(critter);
                    go.transform.position = spawnLocation;
                }
                else
                {
                    GameObject go = Instantiate(predator);
                    go.transform.position = spawnLocation;
                }
            }
        }
    }
}