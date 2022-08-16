using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Kevin
{
    public class ClawRange : MonoBehaviour
    {
        [SerializeField] CritterA parentScript; 
    
        void Awake()
        {
            parentScript = GetComponentInParent<CritterA>();
            SphereCollider sphereCollider = this.GetComponent<SphereCollider>();
            sphereCollider.radius = parentScript.visionRadius/parentScript.visionRadius;
            sphereCollider.center = new Vector3(0, 0, parentScript.visionCenterZ/parentScript.visionCenterZ);
        }

        private void OnTriggerEnter(Collider other)
        {
            parentScript.ActionRange(other);
        }
        
        private void OnTriggerExit(Collider other)
        {
            parentScript.OutOfActionRange(other);
        }
    }
}

