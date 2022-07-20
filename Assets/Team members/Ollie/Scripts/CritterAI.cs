using System;
using System.Collections;
using System.Collections.Generic;
using Tanks;
using UnityEngine;
using Random = System.Random;

namespace Ollie
{
    public class CritterAI : MonoBehaviour
    {
        public float health;
        public float maxHealth;
        public float energy;
        public float maxEnergy;
        public float mySpeed;
        public Vector3 targetLocation;
        public float moveTime;
        public float moveSpeed;
        public float sleepTime;
        
        public Vector3 targetPos;
        public GameObject target;
        public GameObject currentTarget = null;
        public RaycastHit hitData;
        public List<Vector3> path;


        public List<GameObject> targetsInSight;
        private bool rayCooldown;
        private bool interactingTarget;
        private bool foundTarget;
        public bool sleeping;
        public CritterTrigger trigger;
        public Material shader;

        public LevelManager lm;
        public AStar aStar;
        public WaterNode currentLocation;

        private void Start()
        {
            rayCooldown = false;
            health = maxHealth;
            energy = maxEnergy;
            foundTarget = false;
            interactingTarget = false;
            shader = GetComponent<Renderer>().material;
        }

        private void Update()
        {
            if (rayCooldown == false)
            {
                StartCoroutine(RayCoroutine());
            }
            LookForTargets();
            StatChanges();
            shader.SetFloat("_energy",energy);
            
            currentLocation = lm.ConvertToGrid(transform.position);
        }

        private void FixedUpdate()
        {
            //StandardMovement();
            if (path.Count > 0)
            {
                if (transform.position != path[0])
                {
                    transform.position = Vector3.MoveTowards(transform.position,path[0],moveSpeed);
                }
                else if (transform.position == path[0])
                {
                    path.Remove(path[0]);
                }
            }
        }

        public IEnumerator RayCoroutine()
        {
            //clears all targetsInSight every second
            //then runs through the list of all npcs inside trigger zone
            //for each, it assigns them as target, aims ray at them
            //fires ray, stores target in targetsInSight list
            
            //I think this MIGHT pose a problem for targeting behaviours if all targets are briefly removed every second???
            targetsInSight.Clear();
            rayCooldown = true;
            yield return new WaitForSeconds(1);

            for (int i = 0; i < trigger.npcTargets.Count; i++)
            {
                target = trigger.npcTargets[i];
                targetPos = trigger.npcTargets[i].transform.position;
                if (target != null)
                {
                    Ray ray = new Ray(transform.position, (targetPos-transform.position).normalized);
                    Debug.DrawRay(ray.origin, ray.direction*hitData.distance,Color.blue,1f);
                    if (Physics.Raycast(ray, out hitData))
                    {
                        if (hitData.transform.gameObject.GetComponent<iNPC>() != null)
                        {
                            if (!targetsInSight.Contains(hitData.transform.gameObject))
                            {
                                targetsInSight.Add(hitData.transform.gameObject);
                                currentTarget = targetsInSight[UnityEngine.Random.Range(0, targetsInSight.Count)];

                                if (interactingTarget == false)
                                {
                                    MoveTowards(currentTarget);
                                }
                            }
                        }
                        else
                        {
                            currentTarget = null;
                        }
                    }
                }
            }
            rayCooldown = false;
        }

        public void LookForTargets()
        {
            
        }

        #region Colour Changes
        public void ChangeColourGreen()
        {
            this.gameObject.GetComponent<Renderer>().material.color = Color.green;
        }
        
        public void ChangeColourRed()
        {
            this.gameObject.GetComponent<Renderer>().material.color = Color.red;
        }

        public void ChangeColourBlue()
        {
            this.gameObject.GetComponent<Renderer>().material.color = Color.blue;
        }

        public void ChangeColourYellow()
        {
            this.gameObject.GetComponent<Renderer>().material.color = Color.yellow;
        }
        #endregion

        public void StandardMovement()
        {
            transform.position =
                Vector3.MoveTowards(transform.position, targetLocation, (moveSpeed));
            
            if (transform.position == targetLocation && !interactingTarget)
            {
                StartCoroutine(RandomLocation());
            }
            
            
        }

        public IEnumerator RandomLocation()
        {
            targetLocation = new Vector3((UnityEngine.Random.Range(-40,40)), 1, (UnityEngine.Random.Range(-40,40)));
            yield return new WaitForSeconds(moveTime);
            print("noLocationSet = true");
        }

        public void MoveTowards(GameObject newTarget)
        {
            StopCoroutine(RandomLocation());
            targetLocation = newTarget.transform.position;
            if (interactingTarget == false)
            {
                StartCoroutine(InteractTarget(newTarget));
            }
        }

        public IEnumerator InteractTarget(GameObject targetToInteract)
        {
            interactingTarget = true;
            yield return new WaitForSeconds(3f);
            currentTarget = null;
            CheckTarget(targetToInteract);
            if (trigger.npcTargets.Contains(targetToInteract))
            {
                trigger.npcTargets.Remove(targetToInteract);
            }

            if (targetsInSight.Contains(targetToInteract))
            {
                targetsInSight.Remove(targetToInteract);
            }
            
            //logic for how to respond to targets
            if (targetToInteract.GetComponent<iFood>() != null)
            {
                Destroy(targetToInteract);
            }
            
            if (targetToInteract.GetComponent<iPredator>() != null)
            {
                Fight(targetToInteract);
            }

            if (targetToInteract.GetComponent<iCritter>() != null)
            {
                Mate(targetToInteract);
            }
            
            interactingTarget = false;
            if (sleeping == false)
            {
                targetLocation = new Vector3((UnityEngine.Random.Range(-40,40)), 1, (UnityEngine.Random.Range(-40,40)));
            }
            yield return new WaitForSeconds(moveTime);
        }

        private void Fight(GameObject target)
        {
            
        }

        private void Mate(GameObject target)
        {
            //if (target.GetComponent<Critter>().gender == myGender) return;
            if (target.GetComponent<Critter>().gender == Critter.Gender.NonBinary)
            {
                print("target is NB");
            }
        }
        
        public void StatChanges()
        {
            health -= 1 * Time.deltaTime;
            if (health > maxHealth)
            {
                health = maxHealth;
            }

            if (sleeping)
            {
                energy += 10 * Time.deltaTime;
            }
            else
            {
                energy -= 2 * Time.deltaTime;
            }
            
            if (energy > maxEnergy)
            {
                energy = maxEnergy;
            }
            if(energy <= 0)
            {
                energy = 0;
            }
        }

        public void StopCoroutines()
        {
            StopAllCoroutines();
            //currently this is stopping the Destroy part of InteractTarget
            //so main critter goes to consume food/mate/predator but then falls asleep and the obj doesn't die
            
            //this is also breaking the raycast!
            //FIX IT
        }

        public IEnumerator SleepCoroutine()
        {
            sleeping = true;
            print("sleeping");
            targetLocation = transform.position;
            interactingTarget = true;
            yield return new WaitForSeconds(sleepTime);
            interactingTarget = false;
            sleeping = false;
        }

        public void CheckTarget(GameObject checkTarget)
        {
            if (checkTarget.GetComponent<iCritter>() != null)
            {
                health += 5;
                energy -= 5;
            }
            
            if (checkTarget.GetComponent<iFood>() != null)
            {
                health += 5;
            }

            if (checkTarget.GetComponent<iPredator>() != null)
            {
                health -= 10;
                energy -= 5;
            }
        }

        #region Bool Checks for BT
        public bool CheckPredator()
        {
            if (currentTarget.GetComponent<NPCBehaviour>().npcType == NPCBehaviour.NpcType.Predator)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        
        public bool CheckFood()
        {
            if(currentTarget.GetComponent<NPCBehaviour>().npcType == NPCBehaviour.NpcType.Food)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        
        public bool CheckMate()
        {
            if (currentTarget.GetComponent<NPCBehaviour>().npcType == NPCBehaviour.NpcType.Critter)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion
    }
}
