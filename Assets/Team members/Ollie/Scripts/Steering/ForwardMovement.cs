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
        public float speed;

        void Start()
        {
            rigidbody = GetComponentInParent<Rigidbody>();
        }

        void FixedUpdate()
        {
            MoveForward();
        }

        void MoveForward()
        {
            if (rigidbody.velocity.magnitude < 3)
            {
                rigidbody.AddRelativeForce(Vector3.forward * speed);
            }
        }
    }
}
