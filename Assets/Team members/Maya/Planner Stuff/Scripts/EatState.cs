using System.Collections;
using System.Collections.Generic;
using Anthill.AI;
using UnityEngine;

namespace Maya
{ 
    public class EatState : AIBase
    {

        public float eatTimer;
        public float timer;
        
        public override void Enter()
        {
            base.Enter();
            eatTimer = myTouch.timeToEat;
        }

        public override void Execute(float aDeltaTime, float aTimeScale)
        {
            base.Execute(aDeltaTime, aTimeScale);
            timer += aDeltaTime;
            if (timer >= eatTimer && myTouch.isNearFood)
            {
                myAgent.velocity = Vector3.zero;
                timer = 0;
                myEnergy.energyAmount += (eatTimer * 2);
                myTouch.isNearFood = false;

            }
            else
            {

            }

        }

        public override void Exit()
        {
            base.Exit();
            Destroy(myTouch.foodImTouching);
            myVision.foodIveSeen.Remove(myTouch.foodImTouching);
            Finish();
        }
    }
}