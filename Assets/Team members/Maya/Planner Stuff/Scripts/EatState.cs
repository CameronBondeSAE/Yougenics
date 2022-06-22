using System.Collections;
using System.Collections.Generic;
using Anthill.AI;
using UnityEngine;

namespace Maya
{ 
    public class EatState : AntAIState
    {
        public Touch myTouch;
        public Food food;
        public float eatTimer;
        public float timer;

        public override void Create(GameObject aGameObject)
        {
            base.Create(aGameObject);
            myTouch = aGameObject.GetComponent<Touch>();
        }
        public override void Enter()
        {
            base.Enter();
            eatTimer = food.energyValue / 2;
        }

        public override void Execute(float aDeltaTime, float aTimeScale)
        {
            base.Execute(aDeltaTime, aTimeScale);
            timer += aDeltaTime;
            if (timer >= eatTimer)
            {
                myTouch.isNearFood = true;
                timer = 0;
            }
            else
            {
                myTouch.isNearFood = false;
            }

        }

        public override void Exit()
        {
            base.Exit();
            Finish();
        }
    }
}