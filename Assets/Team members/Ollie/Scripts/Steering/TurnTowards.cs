using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ollie
{
    public class TurnTowards : MonoBehaviour
    {
        private Rigidbody rigidbody;
        private Transform parentTransform;
        public Vector3 targetTransform;
        public Vector3 dir;
        public float turnSpeed;
        
        void Start()
        {
            rigidbody = GetComponentInParent<Rigidbody>();
            parentTransform = GetComponentInParent<Transform>();
        }
        
        void FixedUpdate()
        {
            //dir = parentTransform.InverseTransformPoint(targetTransform);
            //TurnParent(dir);
        }

        public void TurnParent(Vector3 targetTransform)
        {
            dir = parentTransform.InverseTransformPoint(targetTransform);
            rigidbody.AddRelativeTorque(0,dir.x*turnSpeed,0,ForceMode.Impulse);
        }
    }
}
