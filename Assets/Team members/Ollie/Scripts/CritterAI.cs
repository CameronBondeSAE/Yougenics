using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ollie
{
    public class CritterAI : MonoBehaviour
    {
        public float health;
        public float maxHealth;
        public float mySpeed;
        
        public Vector3 targetPos;
        public GameObject target;
        public GameObject currentTarget = null;
        public RaycastHit hitData;
        
        public List<GameObject> npcTargets;
        public List<GameObject> targetsInSight;
        private bool rayCooldown;

        private void Start()
        {
            rayCooldown = false;
            health = maxHealth;
        }

        private void Update()
        {
            if (rayCooldown == false)
            {
                StartCoroutine(RayCoroutine());
            }

            LookForTargets();
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
        
        public void ChangeColourRed()
        {
            this.gameObject.GetComponent<Renderer>().material.color = Color.red;
        }

        public void ChangeColourBlue()
        {
            this.gameObject.GetComponent<Renderer>().material.color = Color.blue;
        }
    }
}
