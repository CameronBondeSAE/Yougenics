using System;
using System.Collections;
using System.Collections.Generic;
using Luke;
using UnityEngine;
using UnityEngine.ProBuilder;

namespace Ollie
{
    public class ForwardMovement : MonoBehaviour
    {
        private Rigidbody rigidbody;
        private CritterAIPlanner parent;
        private float speed;

        void Start()
        {
            rigidbody = GetComponentInParent<Rigidbody>();
            parent = GetComponentInParent<CritterAIPlanner>();
        }

        void FixedUpdate()
        {
            MoveForward();
        }

        void MoveForward()
        {
            if (rigidbody.velocity.magnitude < parent.moveSpeed)
            {
                rigidbody.AddRelativeForce(Vector3.forward * parent.moveSpeed);
            }
        }
    }
}
