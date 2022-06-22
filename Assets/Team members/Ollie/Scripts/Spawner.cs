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

        private int planeWidth = 80;
        private int planeLength = 80;
        private int planeFloor = 1;
        
        void Awake()
        {
            CreateNPCs(spawnAmount);
        }

        
        void Update()
        {

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
                spawnLocation = new Vector3((UnityEngine.Random.Range((-planeWidth/2),(planeWidth/2))),planeFloor,(UnityEngine.Random.Range((-planeLength/2),(planeLength/2))));
                
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