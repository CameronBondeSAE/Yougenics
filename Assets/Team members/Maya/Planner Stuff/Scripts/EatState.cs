using System.Collections;
using System.Collections.Generic;
using Anthill.AI;
using UnityEngine;

namespace Maya
{ 
    public class EatState : AIBase
    {
        public Food foodImEating;
        public float eatTimer;
        public float timer;
        
        public override void Enter()
        {
            base.Enter();
            eatTimer = myTouch.timeToEat;
            UpdateEnergyDrainAmount();
            myEnergy.drainAmount = energyBySpeed;
            StartCoroutine(Chomping());
        }

        public override void Execute(float aDeltaTime, float aTimeScale)
        {
            base.Execute(aDeltaTime, aTimeScale);
            timer += aDeltaTime;
            if (timer >= eatTimer && myTouch.isNearFood)
            {
                myAgent.velocity = Vector3.zero;
                Chomping();
            }
        }

        public override void Exit()
        {
            base.Exit();
            //Destroy(myTouch.foodImTouching);
            myVision.foodIveSeen.Remove(myTouch.foodImTouching);
            myEnergy.drainAmount = defaultEnergyDrain;
            Finish();
        }

        private IEnumerator Chomping()
        {
            while (eatTimer > 0)
            {
                yield return new WaitForSeconds(1);
                {
                    float energyPerBite = myEnergy.energyAmount += foodImEating.energyValue / eatTimer;
                }
            }
        }
    }
}