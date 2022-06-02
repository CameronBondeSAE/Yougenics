using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ollie
{
    public class CritterTrigger : MonoBehaviour
    {
        public CritterAI parent;
        
        private void OnTriggerEnter(Collider other)
        {
            iNPC iNPC = other.gameObject.GetComponent<iNPC>();
            if (iNPC != null)
            {
                parent.npcTargets.Add(other.gameObject);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            iNPC iNPC = other.gameObject.GetComponent<iNPC>();
            if (iNPC != null)
            {
                parent.npcTargets.Remove(other.gameObject);
            }
        }
    }
}
