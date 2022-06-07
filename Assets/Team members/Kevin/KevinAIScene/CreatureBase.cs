using System.Collections;
using System.Collections.Generic;
using Kevin;
using NodeCanvas.Tasks.Actions;
using UnityEngine;

namespace Kevin
{
    public class CreatureBase : MonoBehaviour, INpc
    {

        public float healthPoints;
        public float energyPoints;

        public float insightRange;
        public float attackRange;
        
        public bool entityInSightRange;
        public bool entityInAttackRange;
        public bool energyLow;

        public bool isPatrolling;
        public bool isChasing;
        public bool isAttacking;
        public bool isSleeping;
        
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
        }

        public void Chase()
        {
            Debug.Log("Chasing");
            
            //change this logic to a moving towards target logic
            this.transform.position = GetClosestTargetEntity(surroundingEntities, this.transform).position;
            
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

        IEnumerator EnergyRecovery()
        {
            yield return new WaitForSeconds(10f);
            energyPoints = 100f;
        }
    }
}

