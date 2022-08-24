using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Anthill.AI;
using Cam;
using Minh;
using Unity.VisualScripting;
using UnityEngine;
using Object = System.Object;
using Random = UnityEngine.Random;

public class CritterA : CreatureBase, IEdible, ISense
    {
        //Critter Stats
        public float myDangerLevel;

        public float energyTest = 100f;

        public float healthTest = 100f;
        
        //Movement Variables
        public Rigidbody rb;
        public float acceleration;
        public float walkPointRange;
        public float maxSpeed;
        public Vector3 walkPoint;
        public Transform destinationPoint;
        public bool walkPointSet;
        
        
        //Vision Variables
        public float visionRadius = 8f;
        public float visionCenterZ = 2.5f;

        public bool inActionRange;
        
        //List of Entities
        public List<Transform> predatorList;
        public List<Transform> mateList;
        public List<Transform> foodList;
        public Transform closestPredator;
        public Transform closestMate;
        public Transform closestFood;

        
        
        //Separate Components
        [SerializeField] private CommonAttributes commonAttributes;
        [SerializeField] private Energy energy;
        [SerializeField] private Health health;
        
        //Planner Bools
        public bool isHungry;
        public bool isEating;
        public bool isInDanger;
        public bool isSleeping;
        public bool canHunt;
        public bool caughtFood;
        public bool isHealthy;
        public bool readyToMate;
        public bool foundMate;
        public bool isSafe;
        public bool isMating;
        public bool enemyNearby;
        public bool isTired;
        public bool noHealth;
        public bool isDead;
        
        //View Code
        public Renderer renderer;
        public List<GameObject> fireObjects;
        public List<Material> fireMaterial;
        public int currentState;
        public override void Awake()
        {
            commonAttributes = GetComponent<CommonAttributes>();
            rb = GetComponent<Rigidbody>();
        }
        public void OnEnable()
        {
            maxAge = 0f;
            gestationTime = 0f;
            litterSizeMax = 0;
            metabolism = 0f;
            mutationRate = 0f;
            empathy = 0f;
            aggression = 0f;
            sizeScale = 0f;
            colour = new Color(Random.Range(0f, 10f),Random.Range(0f, 10f),Random.Range(0f, 10f));
            sex = (Sex) Random.Range(0, 1);
            commonAttributes.dangerLevel =  Random.Range(0, 10);
            //myDangerLevel = commonAttributes.dangerLevel;
            myDangerLevel = 5;
            //commonAttributes.allegiances = "critter, player, etc"
            //spawnPosition = transform;
            renderer = GetComponent<Renderer>();
        }

        public void FireEmotionsUpdate()
        {
            fireObjects[0].GetComponent<MeshRenderer>().material = fireMaterial[currentState];
            fireObjects[1].GetComponent<MeshRenderer>().material = fireMaterial[currentState];
            fireObjects[2].GetComponent<MeshRenderer>().material = fireMaterial[currentState];
        }
        

        #region CritterExecutables

        #region Wandering/Running Functions

        /*public Vector3 randomPosition;
        public Transform spawnPosition;
        public void Wander()
        {
            if (!walkPointSet)
            {
                walkPoint = GenerateWalkPoint();
            }
            
            if (walkPointSet)
            {
                TurnTo(walkPoint);
                transform.position = Vector3.MoveTowards(transform.position, walkPoint, 0.05f/2f);
                rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxSpeed);
                Vector3 distanceToWalkPoint = transform.position - walkPoint;
                
                if (distanceToWalkPoint.magnitude < 1f)
                {
                    walkPointSet = false;
                }
            }
        }

        private Vector3 GenerateWalkPoint()
        {
            float randomX = Random.Range(-walkPointRange, walkPointRange);
            float randomZ = Random.Range(-walkPointRange, walkPointRange);
            
            walkPoint = new Vector3(transform.position.x + randomX, transform.position.y,
                transform.position.z + randomZ);
            
            walkPointSet = true;
            
            return walkPoint;
        }*/

        public void TurnTo(Vector3 target)
        {
            /*float angle = Vector3.SignedAngle(transform.forward, target - transform.position, Vector3.up);
            rb.AddRelativeTorque(new Vector3(0,Mathf.Sign(angle)*3f,0));*/
            
            Quaternion targetRotation = Quaternion.LookRotation(target - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation,Time.deltaTime);
        }

        /*public void RunAway()
        {
            if (!isSleeping && !isSafe)
            {
                if (predatorList.Count != 0)
                {
                    GenerateWalkPoint();
                    TurnTo(-(GetClosestPredator(predatorList,this.transform).transform.position));
                    transform.position = Vector3.MoveTowards(this.transform.position, 
                        -(GetClosestPredator(predatorList,this.transform).transform.position),-1 * 0.05f);
                }
            }
        }*/

        #endregion

        #region Hunting Functions

        public void HuntFood()
        {
            if (!canHunt && caughtFood) return;
            
            if (foodList.Count != 0)
            {
                TurnTo(GetClosestPrey(foodList,this.transform).transform.position);
                transform.position = Vector3.MoveTowards(this.transform.position, 
                    GetClosestPredator(foodList,this.transform).transform.position,0.075f);
            }
                //Chase after food
            
            
            //if distance is close enough 
            //EatFood();
        }

        public void EatFood()
        {
            if (caughtFood)
            {
                //eating logic
                isEating = true;
            }
            //timer for when finished eating
            //isHungry = false;
            //isEating = false;
            //caughtFood = false;
            //canHunt = false; 
        }

        

        #endregion
     
        #region Mating Functions

        public void MoveTowardsMate()
        {
            if (mateList.Count != 0 && isMating == false)
            {
                TurnTo(GetClosestMate(mateList,this.transform).transform.position);
                transform.position = Vector3.MoveTowards(this.transform.position, 
                    GetClosestPredator(mateList,this.transform).transform.position,0.075f);
            }

            RaycastHit hitInfo;
            if (Physics.Raycast(transform.position,transform.TransformDirection(GetClosestMate(mateList,this.transform).position),out hitInfo ,2f,255, QueryTriggerInteraction.Collide))
            {
                Debug.DrawRay(transform.position, transform.position * hitInfo.distance, Color.red);
                isMating = true;
            }
        }

        public void Mate()
        {
            //mate functions
        }

        #endregion

        #region Sleeping Functions

        public void Sleep()
        {
            
        }

        #endregion

        #region Death Functions

        public void Dead()
        {
            
        }

        #endregion
        
        
        
        #endregion

        #region ListManager

        public void ActionRange(Collider other)
        {
            CreatureBase otherCreatureBase = other.GetComponent<CreatureBase>();
            IEdible otherEdible = other.GetComponent<IEdible>();

            if (otherCreatureBase != null || otherEdible != null)
            {
                inActionRange = true;
            }
        }

        public void OutOfActionRange(Collider other)
        {
            inActionRange = false;
        }
        public void Profiler(Collider other)
        {
            //Debug.Log("vision works");
            CreatureBase otherCreatureBase = other.GetComponent<CreatureBase>();
            IEdible otherEdible = other.GetComponent<IEdible>();
            CommonAttributes otherCommonAttributes = other.GetComponent<CommonAttributes>();
            
            //CritterA otherCritter = other.GetComponent<CritterA>();

            //if (otherCritter == null || otherEdible == null) return;
            
            if (otherCreatureBase != null || otherEdible != null)
            {
                //Predator List
                if (otherCreatureBase != null && sex == otherCreatureBase.sex && myDangerLevel < otherCommonAttributes.dangerLevel)
                {
                    predatorList.Add(other.transform);
                }
                
                //Mate List
                if (otherCreatureBase != null && otherCreatureBase.sex != sex)
                {
                    mateList.Add(other.transform);
                }
                
                //need an If statement for when they are the same sex and same danger level 
                //maybe check for aggression level or just fight it out
            
                //Food List
                if (otherEdible != null)
                {
                    //normal food that doesnt have common attributes component
                    if (otherCommonAttributes == null)
                    {
                        foodList.Add(other.transform);
                    }
                    //other critters with danger level
                    else if (myDangerLevel > otherCommonAttributes.dangerLevel)
                    {
                        foodList.Add(other.transform);
                    }
                }
            }
        }
        
        public void VisionExit(Collider other)
        {
            ListRemover(other.transform);
        }
        
        public void ListRemover(Transform _transform)
        {
            if (predatorList.Contains(_transform))
            {
                predatorList.Remove(_transform);
            }
            if (mateList.Contains(_transform))
            {
                mateList.Remove(_transform);
            }
            if (foodList.Contains(_transform))
            {
                foodList.Remove(_transform);
            }
        }

        #endregion
        

        #region NearestEntities

        Transform GetClosestPredator(List<Transform> predators, Transform thisCritter)
        {
            Transform closestTarget = null;
            float closestDistanceSquared = Mathf.Infinity;
            Vector3 currentPosition = thisCritter.position;
            foreach (Transform potentialTarget in predators)
            {
                Vector3 directionToTarget = potentialTarget.position - currentPosition;
                float distanceSquareToTarget = directionToTarget.sqrMagnitude;
                if (distanceSquareToTarget < closestDistanceSquared)
                {
                    closestDistanceSquared = distanceSquareToTarget;
                    closestTarget = potentialTarget; 
                }
            }
            return closestTarget;
        }
        
        Transform GetClosestPrey(List<Transform> prey, Transform thisCritter)
        {
            Transform closestTarget = null;
            float closestDistanceSquared = Mathf.Infinity;
            Vector3 currentPosition = thisCritter.position;
            foreach (Transform potentialTarget in prey)
            {
                Vector3 directionToTarget = potentialTarget.position - currentPosition;
                float distanceSquareToTarget = directionToTarget.sqrMagnitude;
                if (distanceSquareToTarget < closestDistanceSquared)
                {
                    closestDistanceSquared = distanceSquareToTarget;
                    closestTarget = potentialTarget; 
                }
            }
            return closestTarget;
        }
        
        Transform GetClosestMate(List<Transform> mate, Transform thisCritter)
        {
            Transform closestTarget = null;
            float closestDistanceSquared = Mathf.Infinity;
            Vector3 currentPosition = thisCritter.position;
            foreach (Transform potentialTarget in mate)
            {
                Vector3 directionToTarget = potentialTarget.position - currentPosition;
                float distanceSquareToTarget = directionToTarget.sqrMagnitude;
                if (distanceSquareToTarget < closestDistanceSquared)
                {
                    closestDistanceSquared = distanceSquareToTarget;
                    closestTarget = potentialTarget; 
                }
            }
            return closestTarget;
        }
        
        /*public bool GetClosestPredator(List<Transform> predator, Transform thisCritter)
        {
            if (predatorList.Count == 0) return false;
            
            closestPredator = null;
            float closestDistanceSquared = Mathf.Infinity;
            Vector3 currentPosition = transform.position;
            foreach (Transform potentialTarget in predator)
            {
                Vector3 directionToTarget = potentialTarget.position - currentPosition;
                float distanceSquareToTarget = directionToTarget.sqrMagnitude;
                if (distanceSquareToTarget < closestDistanceSquared)
                {
                    closestDistanceSquared = distanceSquareToTarget;
                    closestPredator = potentialTarget; 
                }
            }
            return closestPredator;
        }*/

        #endregion
        
        #region IEdible

        public float GetEnergyAmount()
        {
            throw new NotImplementedException();
        }

        public float EatMe(float energyRemoved)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region ISense
        public enum AICritterA
        {
            isHungry = 0,
            isEating = 1,
            isInDanger = 2,
            isSleeping = 3,
            canHunt = 4,
            caughtFood = 5,
            isHealthy = 6,
            readyToMate = 7,
            foundMate = 8,
            isSafe = 9,
            isMating = 10,
            enemyNearby = 11,
            isTired = 12,
            noHealth = 13,
            isDead = 14,
            WanderingBehaviour = 15,
            HungryBehaviour = 16,
            MateBehaviour = 17,
            RunningBehaviour = 18,
            SleepingBehaviour = 19,
            DeathBehaviour = 20
        }
        
        public VisionCritterA visionCritterA;
        
        public void CollectConditions(AntAIAgent aAgent, AntAICondition aWorldState)
        {
            aWorldState.Set(AICritterA.isHungry, IsHungry());
            aWorldState.Set(AICritterA.isEating, IsEating());
            aWorldState.Set(AICritterA.isInDanger, IsInDanger());
            aWorldState.Set(AICritterA.isSleeping, IsSleeping());
            aWorldState.Set(AICritterA.canHunt, CanHunt());
            aWorldState.Set(AICritterA.caughtFood, CaughtFood());
            aWorldState.Set(AICritterA.isHealthy, IsHealthy());
            aWorldState.Set(AICritterA.readyToMate, ReadyToMate());
            aWorldState.Set(AICritterA.foundMate, FoundMate());
            aWorldState.Set(AICritterA.isSafe, IsSafe());
            aWorldState.Set(AICritterA.isMating, IsMating());
            aWorldState.Set(AICritterA.enemyNearby, EnemyNearby());
            aWorldState.Set(AICritterA.isTired, IsTired());
            aWorldState.Set(AICritterA.noHealth, NoHealth());
            aWorldState.Set(AICritterA.isDead, IsDead());
            //aWorldState.Set(AICritterA.WanderingBehaviour, true);
            /*aWorldState.Set(AICritterA.HungryBehaviour, true);
            aWorldState.Set(AICritterA.MateBehaviour, true);
            aWorldState.Set(AICritterA.RunningBehaviour, true);
            aWorldState.Set(AICritterA.SleepingBehaviour, true);
            aWorldState.Set(AICritterA.DeathBehaviour, true);*/
        }

        public bool IsHungry()
        {
            if (energyTest < 50)
            {
                isHungry = true;
            }
            else if (energyTest >= 50)
            {
                isHungry = false;
            }
            return isHungry;
        }

        public bool IsEating()
        {
            if (caughtFood && inActionRange)
            {
                isEating = true;
            }
            else
            {
                isEating = false;
            }
            return isEating;
        }

        public bool IsInDanger()
        {
            if (predatorList.Count < 1)
            {
                isInDanger = false;
            }
            else
            {
                isInDanger = true;
            }
            return isInDanger;
        }

        public bool IsSleeping()
        {
            if (isSleeping)
            {
                isSleeping = true;
            }
            else
            {
                isSleeping = false;
            }
            return isSleeping;
        }

        public bool CanHunt()
        {
            if (isHungry && isEating == false && isSafe)
            {
                canHunt = true; 
            }
            else
            {
                canHunt = false; 
            }
            return canHunt;
        }

        public bool CaughtFood()
        {
            RaycastHit hitInfo;
            if (canHunt && inActionRange)
            {
                caughtFood = true;
            }
            else
            {
                caughtFood = false;
            }
            return caughtFood;
        }

        public bool IsHealthy()
        {
            return isHealthy;
        }

        public bool ReadyToMate()
        {
            if (age > ageOfMatingStart && age < ageOfMatingEnd && isSafe && !isSleeping)
            {
                readyToMate = true;
            }
            else
            {
                readyToMate = false;
            }
            //if age is certain 
            return readyToMate;
        }

        public bool FoundMate()
        {
            if (readyToMate && isSafe && mateList.Count > 0)
            {
                foundMate = true;
            }
            else
            {
                foundMate = false;
            }
            return foundMate;
        }

        public bool IsSafe()
        {
            if (!isInDanger && !isSleeping)
            {
                isSafe = true;
            }
            else 
            {
                isSafe = false;
            }
            return isSafe;
        }

        public bool IsMating()
        {
            if (foundMate && isSafe && inActionRange)
            {
                isMating = true;
            }
            else
            {
                isMating = false;
            }
            return isMating;
        }

        public bool EnemyNearby()
        {
            /*if (predatorList.Count < 0)
            {
                enemyNearby = true;
            }
            else
            {
                enemyNearby = false; 
            }*/
            return enemyNearby;
        }

        public bool IsTired()
        {
            /*if (energyTest < 50f)
            {
                isTired = true;
            }*/
            return isTired;
        }

        public bool NoHealth()
        {
            if (healthTest < 1f)
            { 
                isDead = true;
            }
            return noHealth;
        }

        public bool IsDead()
        {
            if (noHealth)
            {
                isDead = true;
            }
            return isDead;
        }

        #endregion
       
    }


