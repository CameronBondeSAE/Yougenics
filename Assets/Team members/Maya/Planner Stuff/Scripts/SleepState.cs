using System.Collections;
using System.Collections.Generic;
using Anthill.AI;
using UnityEngine;

namespace Maya
{ 
    public class SleepState : AIBase
    {

        public override void Enter()
        {
            base.Enter();
            myAgent.velocity = Vector3.zero;
        }

        public override void Execute(float aDeltaTime, float aTimeScale)
        {
            base.Execute(aDeltaTime, aTimeScale);
            myEnergy.drainAmount = sleepEnergyRegen;

        }

        public override void Exit()
        {
            base.Exit();
            myEnergy.drainAmount = defaultEnergyDrain;
            Finish();
        }
    }
}