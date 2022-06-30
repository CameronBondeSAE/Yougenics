using System.Collections;
using System.Collections.Generic;
using Anthill.AI;
using UnityEngine;
using UnityEngine.AI;

namespace Maya
{ 
    public class FindHornyMateState : AIBase
    {
        public Vector3 newPos;
        public float moveTimer;
        public float moveCooldown;
        public float moveDistance;
        public override void Enter()
        {
            base.Enter();
            
            myAgent.speed = 40;
            myEnergy.drainAmount = energyBySpeed;
            UpdateEnergyDrainAmount();
            myEnergy.drainAmount = energyBySpeed;
        }

        public override void Execute(float aDeltaTime, float aTimeScale)
        {
            base.Execute(aDeltaTime, aTimeScale);
            moveTimer += aDeltaTime;
            if (moveTimer >= moveCooldown)
            {
                newPos = RandomNavSphere(transform.position, moveDistance, -1);
                myAgent.SetDestination(newPos);
                moveTimer = 0;
            }
        }

        public override void Exit()
        {
            base.Exit();
            myEnergy.drainAmount = defaultEnergyDrain;
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