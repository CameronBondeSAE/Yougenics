using System;
using System.Collections;
using System.Collections.Generic;
using Tanks;
using UnityEngine;

namespace Ollie
{
    public class Vision : MonoBehaviour
    {
        public GameObject target;
        public RaycastHit hitData;
        public CritterTrigger trigger;
        public Vector3 targetPos;
        public List<GameObject> targetsInSight;
        public GameObject currentTarget;
        public CritterAIPlanner parent;

        public event Action FoodSpottedEvent;
        public event Action PreySpottedEvent;
        public event Action PredatorSpottedEvent;

        public void VisionRefresh()
        {
            //old ray that just constantly sends it out, if it hits, prints hello repeatedly
            
            // Ray ray = new Ray(transform.position,(target.transform.position-transform.position).normalized);
            // if (Physics.Raycast(ray, out hitData))
            // {
            //     if (hitData.transform == target)
            //     {
            //         print("hello");
            //     }
            // }
            
            
            //copied from Behaviour Tree Critter - the trigger child should be modular
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

                                // if (interactingTarget == false)
                                // {
                                //     MoveTowards(currentTarget);
                                // }
                            }

                            if (hitData.transform.gameObject.GetComponent<iFood>() != null)
                            {
                                parent.AddFoodToList(hitData.transform.gameObject);
                                FoodSpottedEvent?.Invoke();
                            }
                        }
                        else
                        {
                            currentTarget = null;
                        }
                    }
                }
            }
        }
    }
}
