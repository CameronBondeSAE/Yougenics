using System;
using System.Collections;
using System.Collections.Generic;
using Anthill.AI;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace Maya
{ 
    public class FindFoodState : AntAIState
    {
        public Vision myVision;
        private NavMeshAgent agent;
        public static List<Food> foodIWant;
        public float moveTimer;
        public float moveCooldown;
        public float moveDistance;
        

        public override void Create(GameObject aGameObject)
        {
            base.Create(aGameObject);
            myVision = aGameObject.GetComponent<Vision>();
        }

        public override void Enter()
        {
            base.Enter();
            agent = GetComponentInParent<NavMeshAgent>();
            agent.speed = 10;
        }

        public override void Execute(float aDeltaTime, float aTimeScale)
        {
            base.Execute(aDeltaTime, aTimeScale);
            if (myVision.foodIveSeen != null)
            {
                foreach (Food piece in myVision.foodIveSeen)
                {
                    foodIWant.Add(piece);
                }
            }
            else
            {
                moveTimer += aDeltaTime;

                if (moveTimer >= moveCooldown)
                {
                    Vector3 newPos = RandomNavSphere(transform.position, moveDistance, -1);
                    agent.SetDestination(newPos);
                    moveTimer = 0;
                }
            }
        }

        public override void Exit()
        {
            base.Exit();
            Finish();
        }
        
        public static Vector3 RandomNavSphere(Vector3 origin, float distance, int layerMask)
        {
            Vector3 randomDirection = Random.insideUnitSphere * distance;

            randomDirection += origin;

            NavMeshHit navHit;
            NavMesh.SamplePosition(randomDirection, out navHit, distance, layerMask);

            return navHit.position;
        }
    }
}