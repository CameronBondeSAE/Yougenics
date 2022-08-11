using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kevin
{
    public class MoveForward : MonoBehaviour
    {
        private Rigidbody rb;
        public float acceleration;

        void Start()
        {
            rb = GetComponent<Rigidbody>();
        }
        void FixedUpdate()
        {
            rb.AddRelativeForce(Vector3.forward * acceleration);
        }
    }
}

