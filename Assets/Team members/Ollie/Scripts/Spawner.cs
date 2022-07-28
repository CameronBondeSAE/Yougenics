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
        public LevelManager lm;

        public int spawnAmount;

        private Vector3 spawnLocation;
        private int planeFloor = 1;
        
        void Awake()
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
                var rng = UnityEngine.Random.Range(1, 101);
                spawnLocation = new Vector3((UnityEngine.Random.Range((-lm.sizeX/2),(lm.sizeX/2))),planeFloor,(UnityEngine.Random.Range((-lm.sizeZ/2),(lm.sizeZ/2))));
                
                if (rng <= 40)
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