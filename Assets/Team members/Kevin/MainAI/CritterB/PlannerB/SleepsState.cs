using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kevin
{
    public class SleepsState : CritterBAIState
    {
        public override void Enter()
        {
            base.Enter();
            StartCoroutine(critterB.SleepTimer());
        }

        
    }
}

