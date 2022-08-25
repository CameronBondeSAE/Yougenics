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
            if (parentScript != null)
            {
                parentScript.Profiler(other);

            }
            else
                Debug.Log("Parent Script Missing Reference");
        }

        void OnTriggerExit(Collider other)
        {
            parentScript.VisionExit(other);
            if (parentScript != null)
            {
                parentScript.VisionExit(other);

            }
            else
                Debug.Log("Parent Script Missing Reference");
        }

    }

}
