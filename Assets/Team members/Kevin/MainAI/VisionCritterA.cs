using System;
using System.Collections;
using System.Collections.Generic;
using Maya;
using UnityEngine;

namespace Kevin
{
    public class VisionCritterA : MonoBehaviour
    {
        public float visionRadius = 4f;
        public float visionCenterZ = 2.5f;
        [SerializeField] Senses senses;
        [SerializeField] CritterA parentScript; 
    
        void Awake()
        {
            parentScript = GetComponentInParent<CritterA>();
            senses = GetComponentInParent<Senses>();
            SphereCollider sphereCollider = this.GetComponent<SphereCollider>();
            sphereCollider.radius = visionRadius;
            sphereCollider.center = new Vector3(0, 0, visionCenterZ);
        }
    

        void OnTriggerEnter(Collider other)
        {
            CreatureBase isCreature = other.GetComponent<CreatureBase>();
            IEdible isEdible = other.GetComponent<IEdible>();
            senses.isWandering = true; 
            if (isCreature != null || isEdible != null)
            {
                parentScript.Profiler(other);
            }
        }

        void OnTriggerExit(Collider other)
        {
            CreatureBase isCreature = other.GetComponent<CreatureBase>();
            IEdible isEdible = other.GetComponent<IEdible>();
            senses.isWandering = false; 
            if (isCreature != null || isEdible != null)
            {
                parentScript.VisionExit(other);
            }
        }
    }
}
  


