using System.Collections;
using System.Collections.Generic;
using Anthill.AI;
using UnityEngine;

namespace Kevin
{
    public class EatFoodState : AntAIState
    {
        public override void Enter()
        {
            base.Enter();
            Debug.Log("Time to eat");
        }

        public override void Execute(float aDeltaTime, float aTimeScale)
        {
            base.Execute(aDeltaTime, aTimeScale);
            Debug.Log("*eating*");
        }
    }

}
