using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Ollie;
using Unity.VisualScripting;
using UnityEngine;
using Vector3 = System.Numerics.Vector3;


namespace Minh
{


    public class Wheel : MonoBehaviour
    {

        private UnityEngine.Vector3 localVelocity;
        public Rigidbody rb;
        public float wheelRadius;
        public float velocity;
        public bool grounded;
        public float forceall;
        public float adjust1;

        RaycastHit hitinfo;

        public void GiveForce()
        {
            rb.AddForceAtPosition(transform.up * (wheelRadius - hitinfo.distance) * forceall, transform.position);
        }

        public void FixedUpdate()
        {
            ShootRays();
            GiveForce();
            AsymmetricalFriction();
            // TODO
            // Lateral forces/Asymetric friction

        }

        public void AsymmetricalFriction()
        {
            localVelocity = transform.InverseTransformDirection(rb.velocity);
            rb.AddRelativeForce(adjust1,0,0);
        }
        public void ShootRays()
        {
            if (Physics.Raycast(transform.position, -transform.up, out hitinfo, wheelRadius))
            {
                grounded = true;
                GiveForce();
                Debug.DrawLine(transform.position, hitinfo.point, Color.green, 1f);
            }
            else
            {
                grounded = false;
                //Debug.DrawLine(transform.position, -transform.up * (travel + wheelRadius), Color.blue);
            }

        }
    }
}