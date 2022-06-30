using System.Collections;
using System.Collections.Generic;
using Anthill.AI;
using UnityEngine;
using UnityEngine.AI;

namespace Maya
{ 
    public class GoToFoodState : AIBase
    {
        public Vector3 newPos;
        
        public override void Enter()
        {
            base.Enter();
            myAgent.speed = 50;
            UpdateEnergyDrainAmount();
            myEnergy.drainAmount = energyBySpeed;;
        }

        public override void Execute(float aDeltaTime, float aTimeScale)
        {
            base.Execute(aDeltaTime, aTimeScale);
            if (myVision.foodIveSeen.Count > 0)
            {
                newPos = myVision.whereFoodIs[0].position;
                
                myAgent.SetDestination(newPos);
                //.myAgent.velocity = Vector3.zero;
            }
        }

        public override void Exit()
        {
            base.Exit();
            myEnergy.drainAmount = defaultEnergyDrain;
            Finish();
        }
    }
}