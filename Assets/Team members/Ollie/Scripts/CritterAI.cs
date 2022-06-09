using System;
using System.Collections;
using System.Collections.Generic;
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
        
        public List<GameObject> npcTargets;
        public List<GameObject> targetsInSight;
        private bool rayCooldown;
        private bool killingTarget;
        private bool foundTarget;
        public bool sleeping;

        private void Start()
        {
            rayCooldown = false;
            health = maxHealth;
            energy = maxEnergy;
            foundTarget = false;
            killingTarget = false;
        }

        private void Update()
        {
            if (rayCooldown == false)
            {
                StartCoroutine(RayCoroutine());
            }
            LookForTargets();
            StatChanges();
        }

        private void FixedUpdate()
        {
            StandardMovement();
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

            for (int i = 0; i < npcTargets.Count; i++)
            {
                target = npcTargets[i];
                targetPos = npcTargets[i].transform.position;
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

                                if (killingTarget == false)
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
            
            if (transform.position == targetLocation && !killingTarget)
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
            if (killingTarget == false)
            {
                StartCoroutine(KillTarget(newTarget));
            }
        }

        public IEnumerator KillTarget(GameObject targetToKill)
        {
            print("killing target");
            killingTarget = true;
            yield return new WaitForSeconds(3f);
            currentTarget = null;
            CheckTarget(targetToKill);
            if (npcTargets.Contains(targetToKill))
            {
                npcTargets.Remove(targetToKill);
            }

            if (targetsInSight.Contains(targetToKill))
            {
                targetsInSight.Remove(targetToKill);
            }
            Destroy(targetToKill);
            killingTarget = false;
            targetLocation = new Vector3((UnityEngine.Random.Range(-40,40)), 1, (UnityEngine.Random.Range(-40,40)));
            yield return new WaitForSeconds(moveTime);
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
        }

        public IEnumerator SleepCoroutine()
        {
            sleeping = true;
            targetLocation = transform.position;
            killingTarget = true;
            yield return new WaitForSeconds(sleepTime);
            killingTarget = false;
            sleeping = false;
        }

        public void CheckTarget(GameObject checkTarget)
        {
            if (checkTarget.GetComponent<iMate>() != null)
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
            if (currentTarget.GetComponent<iPredator>() != null)
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
            if (currentTarget.GetComponent<iFood>() != null)
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
            if (currentTarget.GetComponent<iMate>() != null)
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
