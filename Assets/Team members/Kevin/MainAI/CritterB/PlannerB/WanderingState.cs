using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kevin
{
    public class WanderingState : CritterBAIState
    {
        private Rigidbody rb;
        public float acceleration;
        
        public override void Enter()
        {
            base.Enter();
            acceleration = critterB.acceleration;
            rb = critterB.GetComponent<Rigidbody>();
        }

        public override void Execute(float aDeltaTime, float aTimeScale)
        {
            base.Execute(aDeltaTime, aTimeScale);
            rb.AddRelativeForce(Vector3.forward * acceleration);
        }
    }
}

