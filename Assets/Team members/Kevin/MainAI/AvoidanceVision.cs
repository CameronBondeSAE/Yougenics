using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kevin
{
    public class AvoidanceVision : MonoBehaviour
    {
        [SerializeField] BoidBase parentScript;

        public void Start()
        {
            parentScript = GetComponentInParent<BoidBase>(); 
        }

        public void OnTriggerEnter(Collider other)
        {
            BoidBase otherBoid = other.GetComponent<BoidBase>();
            if (otherBoid != null)
            {
                parentScript.AddNeighbour(other);
            }
        }
        
        public void OnTriggerExit(Collider other)
        {
            BoidBase otherBoid = other.GetComponent<BoidBase>();
            if (otherBoid != null)
            {
                parentScript.RemoveNeighbour(other);
            }
        }
    }
}

