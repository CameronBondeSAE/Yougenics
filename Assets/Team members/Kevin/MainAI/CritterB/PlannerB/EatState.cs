using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kevin
{
    public class EatState : CritterBAIState
    {
        public override void Enter()
        {
            base.Enter();
            
        }

        public override void Execute(float aDeltaTime, float aTimeScale)
        {
            base.Execute(aDeltaTime, aTimeScale);
            Debug.Log("Eating!");
        }

        public override void Exit()
        {
            base.Exit();
            critterB.isEating = false;
            critterB.foundFood = false;
            critterB.isHungry = false;
            critterB.caughtFood = false;
        }
    }
}

