using System.Collections;
using System.Collections.Generic;
using Anthill.AI;
using UnityEngine;

namespace Maya
{ 
    public class MoveToEnemyState : AIBase
    {
        public Vector3 newPos;
        public override void Enter()
        {
            base.Enter();
            myAgent.speed = 35;
            UpdateEnergyDrainAmount();
            myEnergy.drainAmount = energyBySpeed;;
        }

        public override void Execute(float aDeltaTime, float aTimeScale)
        {
            base.Execute(aDeltaTime, aTimeScale);
            if (myVision.potentialVictimsIveSeen.Count > 0)
            {
                newPos = myVision.potentialVictimsIveSeen[0].transform.position;
                
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