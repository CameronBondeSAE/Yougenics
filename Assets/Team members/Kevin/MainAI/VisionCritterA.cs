using System;
using System.Collections;
using System.Collections.Generic;
using Maya;
using UnityEngine;


    public class VisionCritterA : MonoBehaviour
    {
        [SerializeField] CritterA parentScript; 
    
        void Awake()
        {
            parentScript = GetComponentInParent<CritterA>();
            SphereCollider sphereCollider = this.GetComponent<SphereCollider>();
            sphereCollider.radius = parentScript.visionRadius;
            sphereCollider.center = new Vector3(0, 0, parentScript.visionCenterZ);
        }
    

        void OnTriggerEnter(Collider other)
        {
            parentScript.Profiler(other);
        }

        void OnTriggerExit(Collider other)
        {
            parentScript.VisionExit(other);
        }
    }

  


