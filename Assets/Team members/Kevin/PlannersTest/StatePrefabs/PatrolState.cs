using System.Collections;
using System.Collections.Generic;
using Anthill.AI;
using UnityEngine;

namespace Kevin
{
    public class PatrolState : AntAIState
    {
        public class EatFoodState : AntAIState
        {
            public GameObject gluttonPrefab;
            public GluttonBase gluttonBase;
            
            
            public bool walkPointSet;
            public Vector3 walkPoint;
            public float walkPointRange = 10f;
            
            public override void Create(GameObject aGameObject)
            {
                base.Create(aGameObject);
                gluttonPrefab = aGameObject;
                gluttonBase = gluttonPrefab.GetComponent<GluttonBase>();
            }

            public override void Enter()
            {
                base.Enter();
                Debug.Log("Patrolling");
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

            private void TurnTo(Vector3 target)
            {
                Quaternion targetRotation = Quaternion.LookRotation(target - transform.position);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation,Time.deltaTime);
            }
        
            /*public void Patrol()
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
            }*/

            public void GenerateNextWalkPoint()
            {
                float randomX = Random.Range(-walkPointRange, walkPointRange);
                float randomZ = Random.Range(-walkPointRange, walkPointRange);

                walkPoint = new Vector3(transform.position.x + randomX, transform.position.y,
                    transform.position.z + randomZ);

                walkPointSet = true;
                //isPatrolling = true;
            }
        }
        
    }
}

