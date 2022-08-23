using System.Collections;
using System.Collections.Generic;
using Kevin;
using Unity.VisualScripting;
using UnityEngine;

namespace Kevin
{
    public class CanHuntState : CritterBAIState
    {
        public override void Enter()
        {
            base.Enter();
            Debug.Log("Hungry!");
        }

        public override void Execute(float aDeltaTime, float aTimeScale)
        {
            base.Execute(aDeltaTime, aTimeScale);
        }
    }
}

