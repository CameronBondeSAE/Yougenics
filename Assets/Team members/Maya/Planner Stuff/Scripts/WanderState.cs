using System.Collections;
using System.Collections.Generic;
using Anthill.AI;
using UnityEngine;
using UnityEngine.AI;

namespace Maya
{ 
    public class WanderState : AIBase
    {
        public float wanderRadius;
        public float wanderTimer;
        public float timer;
        public override void Enter()
        {
            base.Enter();
            timer = wanderTimer;

        }

        public override void Execute(float aDeltaTime, float aTimeScale)
        {
            base.Execute(aDeltaTime, aTimeScale);
            timer += aDeltaTime;

            if (timer >= wanderTimer)
            {
                Vector3 newPos = RandomNavSphere(transform.position, wanderRadius, -1);
                myAgent.SetDestination(newPos);
                timer = 0;
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