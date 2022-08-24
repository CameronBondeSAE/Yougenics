using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.Android;

namespace Kevin
{
    public class Align : SteeringBehaviour
    {
        public Rigidbody rb;
        public SteeringBehaviour steeringBehaviour;
        public CritterB critterB;

        private void Start()
        {
            rb = GetComponent<Rigidbody>();
            critterB = GetComponent<CritterB>();
            steeringBehaviour = GetComponent<SteeringBehaviour>();
        }

        public void FixedUpdate()
        {
            rb.AddRelativeTorque(steeringBehaviour.CalculateMove(critterB.entityList));
        }
    }
}

