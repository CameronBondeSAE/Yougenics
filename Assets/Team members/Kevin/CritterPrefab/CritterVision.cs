using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kevin
{
    public class CritterVision : MonoBehaviour
    {
        public GameObject critterPrefab;
        public CritterAIPrefab critterAIPrefab;
        public float visionRadius = 8f;
        public void Awake()
        {
            //Get the component of the specific critter script from the prefab assigned.
            //Figure out how to not do it this way later
            critterAIPrefab = critterPrefab.GetComponent<CritterAIPrefab>();
            GetComponent<SphereCollider>().radius = visionRadius;
            GetComponent<SphereCollider>().center = new Vector3(0f, 0f, 9f);
            Physics.IgnoreLayerCollision(6,6);
        }
        
        public void OnTriggerEnter(Collider other)
        {
            /*critterBase.entityInVision = true;
            critterBase.isChasing = true;
            critterBase.Profiling(other);*/
        }
        
        public void OnTriggerExit(Collider other)
        {
            /*critterBase.entityInVision = false;
            critterBase.isChasing = false;
            critterBase.VisionExit(other);*/
        }
    }
}

