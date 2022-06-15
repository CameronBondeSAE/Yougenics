using System;
using System.Collections;
using System.Collections.Generic;
using Kevin;
using UnityEngine;

namespace Kev
{
    public class Vision : MonoBehaviour
    {
        public GameObject gluttonPrefab;
        public GluttonBase gluttonBase;
        public bool inSight;
        public void Awake()
        {
            //Get the component of the specific critter script from the prefab assigned.
            //Figure out how to not do it this way later
            gluttonBase = gluttonPrefab.GetComponent<GluttonBase>();
            GetComponent<SphereCollider>().radius = gluttonBase.visionRadius;
            GetComponent<SphereCollider>().center = new Vector3(0f, 0f, 9f);
            Physics.IgnoreLayerCollision(6,6);
        }

        public void OnTriggerEnter(Collider other)
        {
            inSight = true;
        }
        
        public void OnTriggerExit(Collider other)
        {
            inSight = false;
        }

        public bool SpottedFood()
        {
            if (inSight == true)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        
        
    }
}