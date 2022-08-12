using System;
using System.Collections;
using System.Collections.Generic;
using Tanks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace Ollie
{
    public class CritterAIPlanner : MonoBehaviour, iPathable
    {
        public List<Vector3> path;
        public List<Transform> foodLocationList;
        public List<Transform> mateLocationList;
        public float moveSpeed = 0.25f;
        public WaterNode currentLocation;
        public Transform target;
        [FormerlySerializedAs("targetTransform")] public Vector3 targetPos;
        public AStar aStar;
        public float timer;
        public Vector3 facingDirection;
        private Rigidbody rigidbody;

        private WanderSteering wanderSteering;
        private Avoidance avoidance;
        private ForwardMovement forwardMovement;
        private TurnTowards turnTowards;

        #region Bools for planner World State
        public bool isSafe;
        public bool isHungry;
        [HideInInspector] public bool npcNearby;
        [HideInInspector] public bool preyFound;
        [HideInInspector] public bool foodFound;
        [HideInInspector] public bool predatorFound;
        [HideInInspector] public bool mateFound;
        [HideInInspector] public bool findingPrey;
        [HideInInspector] public bool foodLocated;
        [HideInInspector] public bool findingPredator;
        [HideInInspector] public bool mateLocated;
        public bool isHorny;
        public bool healthLow;
        [HideInInspector] public bool runningAway;
        [HideInInspector] public bool inDanger;
        [HideInInspector] public bool foodNearby;
        [HideInInspector] public bool preyNearby;
        [HideInInspector] public bool mateNearby;
        [HideInInspector] public bool predatorNearby;
        [HideInInspector] public bool preyDead;
        #endregion

     

        private void Start()
        {
            path = new List<Vector3>();
            moveSpeed = 0.25f;
            rigidbody = GetComponent<Rigidbody>();
            
            wanderSteering = GetComponentInChildren<WanderSteering>();
            avoidance = GetComponentInChildren<Avoidance>();
            forwardMovement = GetComponentInChildren<ForwardMovement>();
            turnTowards = GetComponentInChildren<TurnTowards>();
            
            //testing purposes only
            //isHungry = true;
            //healthLow = true;
        }

        private void Update()
        {
            timer += Time.deltaTime;
            if (timer >= 2f)
            {
                timer = 0;
                if (target != null)
                {
                    //wanderSteering.enabled = false;
                    //avoidance.enabled = false;
                    //forwardMovement.enabled = false;
                    targetPos = target.position;
                    if (!LevelManager.instance.ConvertToGrid(targetPos).isBlocked)
                    {
                        aStar.FindPath(transform.position, targetPos);
                    }
                }
                else
                {
                    //wanderSteering.enabled = true;
                    //avoidance.enabled = true;
                    //forwardMovement.enabled = true;
                }
                
            }
            
        }

        private void FixedUpdate()
        {
            if (path.Count > 0)
            {
               if (path.Count > 3)
               {
                   if (Vector3.Distance(transform.position, path[3]) > 3)
                   {
                       turnTowards.TurnParent(path[3]);
                   }
                   else
                   {
                       path.Remove(path[0]);
                   }
               }
               else
               {
                   if (Vector3.Distance(transform.position, targetPos) > 3)
                   {
                       turnTowards.TurnParent(targetPos);
                   }
                   else
                   {
                       path.Clear();
                   }
               }
               #region Original "sliding" pathfinding, but with turnTowards final target

               // if (transform.position != path[0])
               // {
               //     turnTowards.TurnParent(targetPos);
               //     transform.position = Vector3.MoveTowards(transform.position,path[0],moveSpeed);
               // }
               // else if (transform.position == path[0])
               // {
               //     path.Remove(path[0]);
               // }

               #endregion
            }
        }

        public void RandomTarget()
        {
            float posX = (Random.Range((-LevelManager.instance.sizeX/2)+LevelManager.instance.offsetX,(LevelManager.instance.sizeX/2))+LevelManager.instance.offsetX);
            float posY = 1; //update this with heights eventually
            float posZ = (Random.Range((-LevelManager.instance.sizeZ/2)+LevelManager.instance.offsetZ,(LevelManager.instance.sizeZ/2))+LevelManager.instance.offsetZ);
            
            targetPos = new Vector3(posX,posY,posZ);
        }

        public void SetTarget(Transform newTarget)
        {
            target = newTarget;
        }

        public void AddFoodToList(GameObject foodLocation)
        {
            if (!foodLocationList.Contains(foodLocation.transform))
            {
                foodLocationList.Add(foodLocation.transform);
            }
        }

        public void AddMateToList(GameObject mateLocation)
        {
            if (!mateLocationList.Contains(mateLocation.transform))
            {
                mateLocationList.Add(mateLocation.transform);
            }
        }

        public void SetFoodLocated(bool toggle)
        {
            foodLocated = toggle;
        }

        public void SetFoodFound(bool toggle)
        {
            foodFound = toggle;
        }

        public void SetMateLocated(bool toggle)
        {
            mateLocated = toggle;
        }
        
        public void SetMateFound(bool toggle)
        {
            mateFound = toggle;
        }

        public void SetIsHungry(bool toggle)
        {
            isHungry = toggle;
        }

        public void SetIsHorny(bool toggle)
        {
            isHorny = toggle;
        }

        #region iPathable Interface - must have "List<Vector3> path"

        public void GeneratePath(WaterNode node)
        {
            path.Add(LevelManager.instance.ConvertToWorld(node));
        }

        public void ClearPath()
        {
            path.Clear();
        }

        #endregion
    }
}