using System;
using System.Collections;
using System.Collections.Generic;
using Kevin;
using UnityEngine;

namespace Kev
{
    public class VisionSphere : MonoBehaviour
    {
        public GameObject critterPrefab;
        public CritterBase critterBase;
        public void Awake()
        {
            //Get the component of the specific critter script from the prefab assigned.
            //Figure out how to not do it this way later
            critterBase = critterPrefab.GetComponent<CritterBase>();
            GetComponent<SphereCollider>().radius = critterBase.visionRadius;
            GetComponent<SphereCollider>().center = new Vector3(0f, 0f, 9f);
            Physics.IgnoreLayerCollision(6,6);
        }

        public void OnTriggerEnter(Collider other)
        {
            critterBase.entityInVision = true;
            critterBase.isChasing = true;
            critterBase.Profiling(other);
        }
        
        public void OnTriggerExit(Collider other)
        {
            critterBase.entityInVision = false;
            critterBase.isChasing = false;
            critterBase.VisionExit(other);
        }
    }
}