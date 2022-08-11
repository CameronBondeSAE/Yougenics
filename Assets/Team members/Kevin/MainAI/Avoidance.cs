using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

namespace Kevin
{
    public class Avoidance : MonoBehaviour
    {
        private Rigidbody rb;
        public float turnForce;
        [SerializeField] private float rayDistance;

        void Start()
        {
            rb = GetComponent<Rigidbody>();
        }
        void FixedUpdate()
        {
            RaycastHit hitInfo;
            if (Physics.Raycast(new Ray(transform.position, transform.forward), out hitInfo, rayDistance, 255, QueryTriggerInteraction.Ignore))
            {
                Debug.DrawRay(transform.position,transform.forward * rayDistance ,Color.red); 
                rb.AddRelativeTorque(0,turnForce,0,ForceMode.Impulse);
            }
            /*else
            {
                rb.AddRelativeForce(0, 0, moveForce, ForceMode.Impulse);
            }*/
        }
    }
}

