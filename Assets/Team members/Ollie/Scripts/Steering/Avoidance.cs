using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

namespace Ollie
{
    public class Avoidance : MonoBehaviour
    {
        private Rigidbody rigidbody;
        private RaycastHit hitData;
        private Vector3 myPos;
        private Transform parentTransform;
        private ForwardMovement forwardMovement;
        
        // Start is called before the first frame update
        void Start()
        {
            rigidbody = GetComponentInParent<Rigidbody>();
            parentTransform = GetComponentInParent<Transform>();
            forwardMovement = GetComponent<ForwardMovement>();
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            myPos = parentTransform.position;
            
            Vector3 rightFOV = Quaternion.Euler(0, 30f, 0) * parentTransform.forward;
            Vector3 leftFOV = Quaternion.Euler(0, -30f, 0) * parentTransform.forward;
            
            Ray rayCenter = new Ray(myPos, parentTransform.forward.normalized);
            Ray rayRight = new Ray(myPos, rightFOV.normalized);
            Ray rayLeft = new Ray(myPos, leftFOV.normalized);
            
            Debug.DrawRay(rayCenter.origin, rayCenter.direction*hitData.distance,Color.blue,0.1f);
            Debug.DrawRay(rayRight.origin, rayRight.direction*hitData.distance,Color.blue,0.1f);
            Debug.DrawRay(rayLeft.origin, rayLeft.direction*hitData.distance,Color.blue,0.1f);
            
            //for loop through 60 rays (x = -30, less than +30)
            //shoot ray
            //if hit, turn away
            
            if (Physics.Raycast(rayCenter, out hitData))
            {
                if (hitData.distance < 2)
                {
                    rigidbody.AddRelativeTorque(0,6,0);
                    rigidbody.AddRelativeForce(Vector3.back*(forwardMovement.speed/2));
                }
                else if (hitData.distance < 4)
                {
                    rigidbody.AddRelativeTorque(0,4,0);
                    rigidbody.AddRelativeForce(Vector3.back*(forwardMovement.speed/2));
                }
            }

            if (Physics.Raycast(rayRight, out hitData))
            {
                if (hitData.distance < 4)
                {
                    rigidbody.AddRelativeTorque(0,-4,0);
                    rigidbody.AddRelativeForce(Vector3.back*(forwardMovement.speed/2));
                }
                else if (hitData.distance < 6)
                {
                    rigidbody.AddRelativeTorque(0,-2,0);
                    rigidbody.AddRelativeForce(Vector3.back*(forwardMovement.speed/2));
                }
            }
            
            if (Physics.Raycast(rayLeft, out hitData))
            {
                if (hitData.distance < 4)
                {
                    rigidbody.AddRelativeTorque(0,4,0);
                    rigidbody.AddRelativeForce(Vector3.back*(forwardMovement.speed/2));
                }
                else if (hitData.distance < 8)
                {
                    rigidbody.AddRelativeTorque(0,2,0);
                    rigidbody.AddRelativeForce(Vector3.back*(forwardMovement.speed/2));
                }
            }
        }
    }
}
