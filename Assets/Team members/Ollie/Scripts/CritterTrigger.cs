using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ollie
{
    public class CritterTrigger : MonoBehaviour
    {
        public CritterAI parent;
        public List<GameObject> npcTargets;
        
        private void OnTriggerEnter(Collider other)
        {
            iNPC iNPC = other.gameObject.GetComponent<iNPC>();
            if (iNPC != null)
            {
                npcTargets.Add(other.gameObject);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            iNPC iNPC = other.gameObject.GetComponent<iNPC>();
            if (iNPC != null)
            {
                npcTargets.Remove(other.gameObject);
            }
        }
    }
}
