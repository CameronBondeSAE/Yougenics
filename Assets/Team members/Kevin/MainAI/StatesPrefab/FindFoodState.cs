using System.Collections;
using System.Collections.Generic;
using Anthill.AI;
using UnityEngine;

namespace Kevin
{
    public class FindFoodState : AntAIState
    {
        public override void Enter()
        {
            base.Enter();
            Debug.Log("Im hungry!");
        }
        

        public override void Execute(float aDeltaTime, float aTimeScale)
        {
            base.Execute(aDeltaTime, aTimeScale);
            Debug.Log("Where the food at?");
        }
    }
}

