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

        private void Update()
        {
            // rotationX = Quaternion.identity.eulerAngles.x;
            // if (rotationX < -15f)
            // {
            //     parent.moveSpeed = 10;
            //     print("facing up a steep hill!");
            // }
            // else parent.moveSpeed = 3;
        }

        void FixedUpdate()
        {
            MoveForward();
        }

        void MoveForward()
        {
            //this isn't working yet so I'm hardcoding a faster push for up hill for now
            
            // RaycastHit hitData;
            // if (Physics.Raycast(rigidbody.transform.position + rigidbody.velocity/10f, Vector3.down, out hitData))
            // {
            //     projectOnPlane = Vector3.ProjectOnPlane(rigidbody.velocity, hitData.normal);
            // }
            //
            // if (rigidbody.velocity.magnitude < parent.moveSpeed)
            // {
            //     Vector3 xAngle = new Vector3(parent.transform.rotation.x,0,0);
            //     rigidbody.AddRelativeForce((Vector3.forward+ (projectOnPlane)) * parent.moveSpeed);
            // }
            
            if (rigidbody.velocity.magnitude < parent.moveSpeed)
            {
                rigidbody.AddRelativeForce(Vector3.forward * parent.moveSpeed);
            }
        }
    }
}
