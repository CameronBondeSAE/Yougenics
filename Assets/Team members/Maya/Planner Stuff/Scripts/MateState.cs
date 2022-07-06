using System.Collections;
using System.Collections.Generic;
using Anthill.AI;
using UnityEngine;

namespace Maya
{ 
    public class MateState : AIBase
    {
        public float mateTimer;
        public float timer;
        public override void Enter()
        {
            base.Enter();
            mateTimer = myTouch.timeToMate;
            UpdateEnergyDrainAmount();
            myEnergy.drainAmount = energyBySpeed;
            
        }

        public override void Execute(float aDeltaTime, float aTimeScale)
        {
            base.Execute(aDeltaTime, aTimeScale);
            timer += aDeltaTime;
            if (timer >= mateTimer && myTouch.isNearMate)
            {
                myAgent.velocity = Vector3.zero;
            }
        }

        public override void Exit()
        {
            base.Exit();
            myVision.potentialMatesIveSeen.Remove(myTouch.mateImTouching);
            myEnergy.drainAmount = defaultEnergyDrain;
            Finish();
        }
    }
}