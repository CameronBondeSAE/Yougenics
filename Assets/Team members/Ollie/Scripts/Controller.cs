using System;
using System.Collections;
using System.Collections.Generic;
using Tanks;
using UnityEngine;

namespace Ollie
{
    public class Controller : MonoBehaviour
    {
        public CritterAIPlanner parent;
        public BoxCollider collider;
        public Vector3 targetLocation;
        public GameObject target;

        private void Awake()
        {
            targetLocation = parent.transform.position;
            
            //turn off collider on spawn
            //only turn it on when searching a specific target
            collider.enabled = !collider.enabled;
        }

        private void OnEnable()
        {
            //CheckFoodLocationEvent += EnableCollider();
        }

        private void OnDisable()
        {
            //CheckFoodLocationEvent -= EnableCollider();
        }

        void FixedUpdate()
        {
            //MoveToTarget(targetLocation);
        }

        void EnableCollider()
        {
            collider.enabled = enabled;
        }

        public void DisableCollider()
        {
            collider.enabled = !enabled;
        }

        public void MoveToTarget(Vector3 targetLoc)
        {
            parent.transform.position = Vector3.MoveTowards(parent.transform.position, targetLoc, parent.moveSpeed);
            
            //check if you're at targetLoc
            //if colliding with Food
            //progress to EatFood state
            //if colliding with Predator/Mate
            //progress to state etc.
        }

        public void StopMovement()
        {
            targetLocation = parent.transform.position;
        }

        private void OnTriggerEnter(Collider other)
        {
            iNPC iNPC = other.gameObject.GetComponent<iNPC>();
            NPCBehaviour.NpcType type = other.gameObject.GetComponent<NPCBehaviour>().npcType;
            if (iNPC != null)
            {
                if (type == NPCBehaviour.NpcType.Food)
                {
                    print("food was where I thought it was!");
                    parent.foodLocationList.Remove(other.transform);
                    StopMovement();
                    target = other.gameObject;
                    parent.SetFoodFound(true);
                }

                if (type != NPCBehaviour.NpcType.Food)
                {
                    //FoodMissingEvent?.Invoke(); maybe????
                }

                if (type == NPCBehaviour.NpcType.Critter)
                {
                    print("mate was where I thought they were");
                    parent.SetMateFound(true);
                    StopMovement();
                    target = other.gameObject;
                }

                if (type == NPCBehaviour.NpcType.Predator)
                {
                    
                }
            }
        }
    }
}