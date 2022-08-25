using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kevin
{
    public class LayEggState : CritterBAIState
    {
        public override void Enter()
        {
            base.Enter();
            critterB.LayEgg();
            critterB.gestationComplete = false;
        }

        public override void Execute(float aDeltaTime, float aTimeScale)
        {
            base.Execute(aDeltaTime, aTimeScale);
            Debug.Log("Laying Eggs");
        }

        public override void Exit()
        {
            base.Exit();
            StartCoroutine(critterB.GestationTimer());
        }
    }
}

