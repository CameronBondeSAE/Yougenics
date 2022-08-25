using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kevin
{
    public class VisionCritterB : MonoBehaviour
    {
        [SerializeField] CritterB parentScript; 
    
        void Awake()
        {
            parentScript = GetComponentInParent<CritterB>();
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

}
