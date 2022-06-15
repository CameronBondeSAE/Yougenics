using System;
using System.Collections;
using System.Collections.Generic;
using Kevin;
using Tanks;
using UnityEngine;

namespace Kevin
{
    public class VisionSphere : MonoBehaviour
    {
        public GameObject critterObject;
        public CritterbaseRaycast critterbaseRaycast;
        //public float sphereRadius;
        void Awake()
        {
            critterbaseRaycast = critterObject.GetComponent<CritterbaseRaycast>();
            //GetComponent<SphereCollider>().radius = sphereRadius;
        }

        public void OnTriggerEnter(Collider other)
        {
            critterbaseRaycast.InVision(other);
        }

        public void OnTriggerExit(Collider other)
        {
            critterbaseRaycast.OutVision(other);
        }
    }
}

