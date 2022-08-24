using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kevin
{
    public class DieState : CritterBAIState
    {
        public override void Enter()
        {
            base.Enter();
            //Dead state
            Debug.Log("I'm Dead!");
            critterB.deathColour = 0f;
            critterB.GetComponent<Wander>().enabled = false;
            critterB.Dead();
        }
    }
}

