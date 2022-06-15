using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Kevin;
using ParrelSync.NonCore;
using Unity.VisualScripting;
using Random = UnityEngine.Random;

namespace Kev
{
    public class CritterBase : MonoBehaviour, Interface.ICritter
    {
        //critter stat variables
        public float health;
        public float energy;
        public Interface.Gender gender;
        public Interface.Foodchain foodChain;
        public float visionRadius = 8f;
        public float actionRadius = 0.25f;
        private float maxHealth = 100f;
        private float maxEnergy = 100f;
        
        //critter state bools
        public bool isPatrolling;
        public bool isChasing;
        public bool isEating;
        public bool isSleeping;
        public bool isMating;
        public bool isRunning;
        
        //conditions for state identification
        //just tests
        public bool entityInVision;
        public bool entityInAttackRange;
        public bool horny;
        public bool hungry;
        public bool sleepy;

        public bool hasMated;
        
        //Transform list for other entities
        public List<Transform> neutralList;
        public List<Transform> predatorList;
        public List<Transform> preyList;
        public List<Transform> potentialMates;

        //public List<Transform> foodList;
        
        //Patrolling Variables
        public bool walkPointSet;
        public Vector3 walkPoint;
        public float walkPointRange;

        //public RaycastHit raycastHit;
        public void Awake()
        {
            health = 100f;
            energy = 100f;
            gender = (Interface.Gender)Random.Range(0f, 2f);
            foodChain = (Interface.Foodchain)Random.Range(0f, 3f);
        }

        public void FixedUpdate()
        {
            //I am straight doodoo at behaviour trees
            
            //these if statements are just for testing
            if (isPatrolling)
            {
                Patrol();
            }

            if (isChasing)
            {
                isPatrolling = false;
                Chase();
            }

            if (isMating)
            {
                BeginMate();
            }
            /*if (health < 1f && isSleeping == false)
            {
                StartCoroutine(Sleep());
                isSleeping = true;
                isPatrolling = false;
            }
            else if(isSleeping == false)
            {
                HealthDecrease();
            }*/
        }

        public void Profiling(Collider other)
        {
            Interface.Foodchain otherFoodChain = other.GetComponent<CritterBase>().foodChain;
            Interface.Gender otherGender = other.GetComponent<CritterBase>().gender;
            RaycastHit raycastHit;
            //Interface.IEdible edible = GetComponent<Interface.IEdible>();
            
            if (otherFoodChain == Interface.Foodchain.Neutral)
            {
                //if(RaycastHit())
                neutralList.Add(other.transform);
            }
            if (otherFoodChain == Interface.Foodchain.Predator && foodChain != Interface.Foodchain.Predator)
            {
                isRunning = true;
                predatorList.Add(other.transform);
            }
            
            if (otherFoodChain == Interface.Foodchain.Prey)
            {
                preyList.Add(other.transform);
            }

            if (otherFoodChain == foodChain && otherGender != gender)
            {
                potentialMates.Add(other.transform);
            }
            else if (otherFoodChain == foodChain && otherGender == gender)
            {
                neutralList.Add(other.transform);
                isChasing = false;
                isPatrolling = true;
            }
            

            /*if (edible != null)
            {
                foodList.Add(other.transform);
            }*/

            /*RaycastHit hit;
            if(Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit))
            {
                Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.red);
                Debug.Log("Hit");
            }
            else
            {
                Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 1000, Color.yellow);
                Debug.Log("Empty");
            }*/
        }

        public void VisionExit(Collider other)
        {
            ListRemover(other.transform);
        }

        public void ListRemover(Transform transform)
        {
            if (neutralList.Contains(transform))
            {
                neutralList.Remove(transform);
            }
            if (predatorList.Contains(transform))
            {
                predatorList.Remove(transform);
            }
            if (preyList.Contains(transform))
            {
                preyList.Remove(transform);
            }
            if (potentialMates.Contains(transform))
            {
                potentialMates.Remove(transform);
            }
        }

        /*public void HealthDecrease()
        {
            //health--;
        }
        
        public void EnergyDecrease()
        {
            
        }

        IEnumerator Sleep()
        {
            yield return new WaitForSeconds(10f);
            health = maxHealth;
            isPatrolling = true;
            isSleeping = false;
        }*/

        private void TurnTo(Vector3 target)
        {
            Quaternion targetRotation = Quaternion.LookRotation(target - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation,Time.deltaTime);
        }
    
        //does not work because if the object turns around so does the collider and it is no longer running away.
        //more tests needed
        /*private void TurnAway(Vector3 target)
        {
            Quaternion targetRotation = Quaternion.LookRotation(transform.position - target);
            transform.rotation = Quaternion.Slerp(targetRotation, transform.rotation,Time.deltaTime);
            //transform.LookAt(new Vector3 (target.x, transform.position.y, target.z));
        }*/
        
        public void Patrol()
        {
            if (!walkPointSet)
            {
                //StartCoroutine(GenerateNextWalkPoint());
                GenerateNextWalkPoint();
                //isPatrolling = false;
            }

            if (walkPointSet)
            {
                TurnTo(walkPoint);
                transform.position = Vector3.MoveTowards(transform.position, walkPoint, 0.05f);
                
                //checks if the distance to the walk point is too short in which case it sets the walkPointSet to false and retries the random generation.
                Vector3 distanceToWalkPoint = transform.position - walkPoint;
                if (distanceToWalkPoint.magnitude < 1f)
                {
                    walkPointSet = false;
                }
            }
        }

        public void GenerateNextWalkPoint()
        {
            float randomX = Random.Range(-walkPointRange, walkPointRange);
            float randomZ = Random.Range(-walkPointRange, walkPointRange);

            walkPoint = new Vector3(transform.position.x + randomX, transform.position.y,
                transform.position.z + randomZ);

            walkPointSet = true;
            //isPatrolling = true;
        }
        
        /*IEnumerator GenerateNextWalkPoint()
        {
            yield return new WaitForSeconds(5f);
            float randomX = Random.Range(-walkPointRange, walkPointRange);
            float randomZ = Random.Range(-walkPointRange, walkPointRange);

            walkPoint = new Vector3(transform.position.x + randomX, transform.position.y,
                transform.position.z + randomZ);

            walkPointSet = true;
            isPatrolling = true;
        }*/

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
        
        Transform GetClosestNeutral(List<Transform> neutral, Transform thisCritter)
        {
            Transform closestTarget = null;
            float closestDistanceSquared = Mathf.Infinity;
            Vector3 currentPosition = thisCritter.position;
            foreach (Transform potentialTarget in neutral)
            {
                Vector3 direactionToTarget = potentialTarget.position - currentPosition;
                float distanceSquareToTarget = direactionToTarget.sqrMagnitude;
                if (distanceSquareToTarget < closestDistanceSquared)
                {
                    closestDistanceSquared = distanceSquareToTarget;
                    closestTarget = potentialTarget; 
                }
            }
            return closestTarget;
        }
        public void Chase()
        {
            walkPointSet = true;

            //StartCoroutine(Hunt());
            //isChasing = false;
            
            //runs away from the nearest predator 
            if (predatorList.Count != 0 && isRunning)
            {
                TurnTo(GetClosestPredator(predatorList,this.transform).transform.position);
                transform.position = Vector3.MoveTowards(this.transform.position, 
                    GetClosestPredator(predatorList,this.transform).transform.position,-1 * 0.05f);
            }
            
            //Chases the nearest neutral
            if (neutralList.Count != 0 && isChasing)
            {
                TurnTo(GetClosestNeutral(neutralList,this.transform).transform.position);
                transform.position = Vector3.MoveTowards(this.transform.position, 
                    GetClosestPredator(neutralList,this.transform).transform.position,0.05f);
            }
            
            //Chases the nearest prey
            if (preyList.Count != 0 && isChasing)
            {
                TurnTo(GetClosestPrey(preyList,this.transform).transform.position);
                transform.position = Vector3.MoveTowards(this.transform.position, 
                    GetClosestPredator(preyList,this.transform).transform.position,0.075f);
            }

            if (potentialMates.Count != 0 && isChasing && isMating)
            {
                TurnTo(GetClosestMate(potentialMates,this.transform).transform.position);
                transform.position = Vector3.MoveTowards(this.transform.position, 
                    GetClosestPredator(potentialMates,this.transform).transform.position,0.075f);
            }
        }

        /*IEnumerator Hunt()
        {
            yield return new WaitForSeconds(5f);
            //runs away from the nearest predator 
            if (predatorList.Count != 0 && isRunning)
            {
                TurnTo(GetClosestPredator(predatorList,this.transform).transform.position);
                transform.position = Vector3.MoveTowards(this.transform.position, 
                    GetClosestPredator(predatorList,this.transform).transform.position,-1 * 0.05f);
            }
            
            //Chases the nearest neutral
            if (neutralList.Count != 0 && isChasing)
            {
                TurnTo(GetClosestNeutral(neutralList,this.transform).transform.position);
                transform.position = Vector3.MoveTowards(this.transform.position, 
                    GetClosestPredator(neutralList,this.transform).transform.position,0.05f);
            }
            
            //Chases the nearest prey
            if (preyList.Count != 0 && isChasing)
            {
                TurnTo(GetClosestPrey(preyList,this.transform).transform.position);
                transform.position = Vector3.MoveTowards(this.transform.position, 
                    GetClosestPredator(preyList,this.transform).transform.position,0.075f);
            }

            isChasing = true;
        }*/

        public void BeginEat(Collider other)
        {
            /*float energyValue = other.GetComponent<Food>().energyValue;
            if (energy != 100f)
            {
                energy += energyValue;
                if (energy > 99f)
                {
                    energy = maxEnergy;
                }
            }*/
        }

        public void BeginMate()
        {
            if (isMating && hasMated == false)
            {
                Debug.Log("Started Mating");
                hasMated = true;
            }
            else
            {
                Debug.Log("Monogamy Mate!!!");
            }
        }
        public void Sleepy()
        {
            throw new NotImplementedException();
        }

        public void Death()
        {
            gameObject.SetActive(false);
            isPatrolling = true;

            /*predatorList.Clear();
            neutralList.Clear();
            preyList.Clear();
            potentialMates.Clear();*/
        }
    }
    
}

