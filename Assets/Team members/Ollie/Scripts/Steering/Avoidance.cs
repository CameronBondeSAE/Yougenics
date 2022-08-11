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
        
        // Start is called before the first frame update
        void Start()
        {
            rigidbody = GetComponent<Rigidbody>();
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            myPos = transform.position;
            Vector3 rightFOV = Quaternion.Euler(0, 30f, 0) * transform.forward;
            Vector3 leftFOV = Quaternion.Euler(0, -30f, 0) * transform.forward;
            
            Ray rayCenter = new Ray(myPos, transform.forward.normalized);
            Ray rayRight = new Ray(myPos, rightFOV.normalized);
            Ray rayLeft = new Ray(myPos, leftFOV.normalized);
            
            Debug.DrawRay(rayCenter.origin, rayCenter.direction*hitData.distance,Color.blue,0.1f);
            Debug.DrawRay(rayRight.origin, rayRight.direction*hitData.distance,Color.blue,0.1f);
            Debug.DrawRay(rayLeft.origin, rayLeft.direction*hitData.distance,Color.blue,0.1f);
            
            if (Physics.Raycast(rayCenter, out hitData))
            {
                if (hitData.distance < 1)
                {
                    rigidbody.AddRelativeTorque(0,5,0);
                }
                else if (hitData.distance < 2)
                {
                    rigidbody.AddRelativeTorque(0,3,0);
                }
            }

            if (Physics.Raycast(rayRight, out hitData))
            {
                if (hitData.distance < 5)
                {
                    rigidbody.AddRelativeTorque(0,-1,0);
                }
            }
            
            if (Physics.Raycast(rayLeft, out hitData))
            {
                if (hitData.distance < 6)
                {
                    rigidbody.AddRelativeTorque(0,1,0);
                }
            }
        }
    }
}
