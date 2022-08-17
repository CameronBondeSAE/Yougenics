using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

namespace Kevin
{
    public class Avoidance : MonoBehaviour
    {
        private Rigidbody rb;
        public float turnForce;
        [SerializeField] private float rayDistance;

        [SerializeField] private Vector3 dirR;
        [SerializeField] private Vector3 dirL;
        void Start()
        {
            rb = GetComponent<Rigidbody>();
        }
        void FixedUpdate()
        {
            Vector3 dirF = transform.forward;
            Vector3 dirR = Quaternion.Euler(0, 30f, 0) * transform.forward;
            Vector3 dirL = Quaternion.Euler(0, -30f, 0) * transform.forward;

            RaycastHit hitInfo;
            RaycastHit hitRight;
            RaycastHit hitLeft;

            if (rayDistance < 2)
            {
                rb.AddRelativeTorque(0,turnForce*2f,0,ForceMode.Impulse);
            }
            
            if (Physics.Raycast(new Ray(transform.position, dirF), out hitInfo, rayDistance, 255, QueryTriggerInteraction.Ignore))
            {
                Debug.DrawRay(transform.position,dirF * rayDistance ,Color.red);
                rb.AddRelativeTorque(0,turnForce,0,ForceMode.Impulse);
            }
            
            if (Physics.Raycast(new Ray(transform.position, dirR), out hitRight, rayDistance, 255, QueryTriggerInteraction.Ignore))
            {
                Debug.DrawRay(transform.position,dirR * rayDistance * 1.5f ,Color.blue);
                rb.AddRelativeTorque(0,-turnForce,0,ForceMode.Impulse);
            }
            
            if (Physics.Raycast(new Ray(transform.position, dirL), out hitLeft, rayDistance, 255,
                         QueryTriggerInteraction.Ignore))
            {
                Debug.DrawRay(transform.position,dirL * rayDistance * 1.5f ,Color.blue);
                rb.AddRelativeTorque(0,turnForce,0,ForceMode.Impulse);
            }
        }
    }
}

