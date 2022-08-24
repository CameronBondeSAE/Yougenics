using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kevin
{
    public class TurnTowards : MonoBehaviour
    {
        public Rigidbody rb;
        public Transform transform;
        public Vector3 dir;
        public float speed;

        public void Start()
        {
            rb = GetComponent<Rigidbody>();
            transform = GetComponent<Transform>();
        }

        public void Turn(Vector3 targetTransform)
        {
            dir = transform.InverseTransformPoint(targetTransform);
            rb.AddRelativeTorque(0, dir.x * speed, 0, ForceMode.Impulse);
        }

    }
}

