using System;
using System.Collections;
using System.Collections.Generic;
using Anthill.AI;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace Maya
{ 
    public class FindFoodState : AIBase
    {
        public Vector3 newPos;
        public List<Food> foodIWant;
        public float moveTimer;
        public float moveCooldown;
        public float moveDistance;
        

        
        public override void Enter()
        {
            base.Enter();
            myAgent.speed = 10;
        }

        public override void Execute(float aDeltaTime, float aTimeScale)
        {
            base.Execute(aDeltaTime, aTimeScale);
            moveTimer += aDeltaTime;
            if (foodIWant == null)
            {
                if (moveTimer >= moveCooldown)
                {
                    newPos = RandomNavSphere(transform.position, moveDistance, -1);
                    myAgent.SetDestination(newPos);
                    moveTimer = 0;
                }
            }
            else if (myVision.foodIveSeen != null)
            {
                foreach (Food piece in myVision.foodIveSeen)
                {
                    foodIWant.Add(piece);
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