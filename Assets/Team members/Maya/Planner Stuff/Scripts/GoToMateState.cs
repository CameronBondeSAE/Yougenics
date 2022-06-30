using System.Collections;
using System.Collections.Generic;
using Anthill.AI;
using UnityEngine;

namespace Maya
{ 
    
    public class GoToMateState : AIBase
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
            if (myVision.potentialMatesIveSeen.Count > 0)
            {
                newPos = myVision.potentialMatesIveSeen[0].transform.position;
                
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