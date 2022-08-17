using System.Collections;
using System.Collections.Generic;
using Anthill.AI;
using UnityEngine;

namespace Kevin
{
    public class MoveToMateState : CritterAAIState
    {
        public override void Enter()
        {
            base.Enter();
        }

        public override void Execute(float aDeltaTime, float aTimeScale)
        {
            base.Execute(aDeltaTime, aTimeScale);
            Debug.Log("chasing soulmate!");
            critterA.MoveTowardsMate();
        }
    }

}
