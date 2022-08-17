using System;
using System.Collections;
using System.Collections.Generic;
using Kevin;
using Minh;
using Tanks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace Ollie
{
    public class CritterAIPlanner : CreatureBase, iPathable, IEdible
    {
        public List<Vector3> path;
        public List<Transform> foodLocationList;
        public List<GameObject> mateLocationList;
        public float moveSpeed = 0.25f;
        public WaterNode currentLocation;
        public Transform target;
        [FormerlySerializedAs("targetTransform")] public Vector3 targetPos;
        public AStar aStar;
        public float timer;
        public Vector3 facingDirection;
        private Rigidbody rigidbody;
        
        public TurnTowards turnTowards;
        private Avoidance avoidance;
        

        private Health healthComponent;
        private Energy energyComponent;
        private StateViewer stateViewer;

        private bool sleeping;
        private bool dead;

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
            moveSpeed = 3;
            rigidbody = GetComponent<Rigidbody>();
            turnTowards = GetComponentInChildren<TurnTowards>();
            avoidance = GetComponentInChildren<Avoidance>();
            stateViewer = GetComponentInChildren<StateViewer>();
            
            healthComponent = GetComponent<Minh.Health>();
            healthComponent.CurrentHealth.Value = healthComponent.maxHealth;
            

            energyComponent = GetComponent<Energy>();
            energyComponent.useEnergyOnMovement = true;
            energyComponent.EnergyAmount.Value = energyComponent.energyMax;


            if (Random.Range(0, 2) == 0)
            {
                sex = Sex.Female;
            }
            else sex = Sex.Male;
            //testing purposes only
            //isHungry = true;
            //healthLow = true;
        }

        private void Update()
        {
            timer += Time.deltaTime;
            if (timer >= 0.1f)
            {
                timer = 0;
                if (target != null)
                {
                    avoidance.ignoreList.Add(target.gameObject);
                    targetPos = target.position;
                    if (!LevelManager.instance.ConvertToGrid(targetPos).isBlocked)
                    {
                        aStar.FindPath(transform.position, targetPos);
                    }
                }
            }

            if (healthComponent.CurrentHealth.Value < 50)
            {
                //SetHealthLow(true);
            }

            if (healthComponent.CurrentHealth.Value <= 0)
            {
                Death();
                
            }
            //else SetHealthLow(false);

            if (energyComponent.EnergyAmount.Value <= energyComponent.energyMin)
            {
                moveSpeed = 0;
                sleeping = true;
            }

            if (sleeping)
            {
                Sleep();
            }

            if (sleeping && dead)
            {
                DestroyMe();
            }
        }

        public void StateViewerChange(int index)
        {
            stateViewer.ChangeParticles(index);
        }

        private void Death()
        {
            dead = true;
            healthComponent.IsDead.Value = true;
            energyComponent.useEnergyOnMovement = false;
            moveSpeed = 0;
        }

        private void Sleep()
        {
            StateViewerChange(4);
            energyComponent.EnergyAmount.Value += 10 * Time.deltaTime;

            if (energyComponent.EnergyAmount.Value >= energyComponent.energyMax)
            {
                energyComponent.EnergyAmount.Value = energyComponent.energyMax;
                sleeping = false;
                moveSpeed = 3;
            }
        }

        private void DestroyMe()
        {
            print(this + " has no health or energy, should be destroyed");
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
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
            if (!mateLocationList.Contains(mateLocation))
            {
                mateLocationList.Add(mateLocation);
            }
        }

        public void SetFoodLocated(bool toggle)
        {
            foodLocated = toggle;
        }

        public void SetHealthLow(bool toggle)
        {
            healthLow = toggle;
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

        #region IEdible Interface

        public float GetEnergyAmount()
        {
            throw new NotImplementedException();
        }

        public float EatMe(float energyRemoved)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}