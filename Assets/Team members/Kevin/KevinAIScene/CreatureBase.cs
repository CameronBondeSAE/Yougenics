using System.Collections;
using System.Collections.Generic;
using Kevin;
using NodeCanvas.Tasks.Actions;
using UnityEngine;

namespace Kevin
{
    public class CreatureBase : MonoBehaviour, INpc, IEntity 
    {
        //Critter Class Variables
        public float healthPoints;
        public float energyPoints;

        //Collider range variables
        public float insightRange;
        public float attackRange;
        public float matingRange;
        
        //Behaviour Tree and State Checkers
        public bool entityInSightRange;
        public bool entityInAttackRange;
        public bool energyLow;
        public bool isPatrolling;
        public bool isChasing;
        public bool isAttacking;
        public bool isSleeping;
        public bool isMating;
        
        //Patrolling Variables
        public bool walkPointSet;
        public Vector3 walkPoint;
        public float walkPointRange;
        
        public  List<Transform> surroundingEntities = new List<Transform>();
        public void Update()
        {
            if (surroundingEntities.Count > 0)
            {
                Debug.Log("Closest to: " + GetClosestTargetEntity(surroundingEntities, this.transform));
            }

            if (energyPoints < 1)
            {
                energyLow = true; 
            }
            
            if(energyLow && energyPoints > 99f)
            {
                energyLow = false;
            }
        }
        
        private void OnDrawGizmosSelected() 
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, insightRange);
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, attackRange);
        }

        Transform GetClosestTargetEntity(List<Transform> entities, Transform thisNpc)
        {
            Transform closestTarget = null;
            float closestDistanceSquared = Mathf.Infinity;
            Vector3 currentPosition = thisNpc.position;
            foreach (Transform potentialTarget in entities)
            {
                Vector3 direactionToTarget = potentialTarget.position - currentPosition;
                float distanceSquareToTarget = direactionToTarget.sqrMagnitude;
                if (distanceSquareToTarget < closestDistanceSquared)
                {
                    closestDistanceSquared = distanceSquareToTarget;
                    closestTarget = potentialTarget; 
                }
            }
            //Debug.Log(closestTarget);
            return closestTarget;
        }

        public void Patrol()
        {
            //patrolling logic goes here
            //create another function that generates a random position in which the critter can use as a target point
            //whilst patrolling these two functions will loop and continue to generate a random target point until a new state is activated/allowed
            Debug.Log("Patrolling");
            isPatrolling = true;
            isChasing = false;
            isAttacking = false;
            isSleeping = false;

            if (!walkPointSet)
            {
                GenerateNextWalkPoint();
            }

            if (walkPointSet)
            {
                transform.position = Vector3.MoveTowards(transform.position, walkPoint, 0.01f);
                
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
            /*//checks if the walk point generated is ground to prevent the enemy from falling off.
            if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
            {
                walkPointSet = true; 
            }*/
        }

        public void Chase()
        {
            Debug.Log("Chasing");
            walkPointSet = true;
            transform.position = Vector3.MoveTowards(this.transform.position, GetClosestTargetEntity(surroundingEntities, this.transform).transform.position,0.01f);
            //change this logic to a moving towards target logic
            //this.transform.position = GetClosestTargetEntity(surroundingEntities, this.transform).position;
            
            isPatrolling = false;
            isChasing = true;
            isAttacking = false;
            isSleeping = false;
            

        }

        public void Attack()
        {
            //attack logic goes here
            
            Debug.Log("Attacking");
            isPatrolling = false;
            isChasing = false;
            isAttacking = true;
            isSleeping = false;
        }

        public void Sleep()
        {
            Debug.Log("Sleeping");
            isPatrolling = false;
            isChasing = false;
            isAttacking = false;
            isSleeping = true;
            StartCoroutine(EnergyRecovery());
        }

        public void Mate()
        {
            throw new System.NotImplementedException();
        }

        IEnumerator EnergyRecovery()
        {
            yield return new WaitForSeconds(10f);
            energyPoints = 100f;
        }
    }
}

