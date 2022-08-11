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
        public float magnitude;

        // Start is called before the first frame update
        void Start()
        {
            rigidbody = GetComponentInParent<Rigidbody>();
        }

        private void Update()
        {
            magnitude = rigidbody.velocity.magnitude;
        }

        // Update is called once per frame
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
