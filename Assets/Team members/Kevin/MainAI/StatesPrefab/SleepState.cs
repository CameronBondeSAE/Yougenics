using System.Collections;
using System.Collections.Generic;
using Anthill.AI;
using UnityEngine;

namespace Kevin
{
    public class SleepState : CritterAAIState
    {
        public override void Enter()
        {
            base.Enter();
            critterA.Sleep();
        }

        public override void Execute(float aDeltaTime, float aTimeScale)
        {
            base.Execute(aDeltaTime, aTimeScale);
            Debug.Log("Im sleeping!!!");
        }
    }
}

