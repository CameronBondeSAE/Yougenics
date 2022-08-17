using System.Collections;
using System.Collections.Generic;
using Anthill.AI;
using UnityEngine;

namespace Kevin
{
    public class WanderingBehaviourState : CritterAAIState
    {
        public bool walkPointSet;
        public Vector3 walkPoint;
        public float walkPointRange = 10f;
        
        public override void Enter()
        {
            base.Enter();
            //Change emotion UI
            critterA.alphaFloat = 1f;
            critterA.currentState = 1;
        }

        public override void Execute(float aDeltaTime, float aTimeScale)
        {
            base.Execute(aDeltaTime, aTimeScale);
            //critterA.Wander();
            
            if (!walkPointSet)
            {
                GenerateNextWalkPoint();
            }

            if (walkPointSet)
            {
                TurnTo(walkPoint);
                critterA.transform.position = Vector3.MoveTowards(transform.position, walkPoint, 0.025f);

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
                critterA.transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation,Time.deltaTime);
            }
            
            public void GenerateNextWalkPoint()
            {
                float randomX = Random.Range(-walkPointRange, walkPointRange);
                float randomZ = Random.Range(-walkPointRange, walkPointRange);

                walkPoint = new Vector3(transform.position.x + randomX, transform.position.y,
                    transform.position.z + randomZ);

                walkPointSet = true;
            }
    }
}

