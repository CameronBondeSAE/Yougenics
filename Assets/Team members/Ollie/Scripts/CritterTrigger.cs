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
        
        //adds anything within range with iNPC interface to a list
        private void OnTriggerEnter(Collider other)
        {
            IEdible iEdible = other.gameObject.GetComponent<IEdible>();
            if (iEdible != null)
            {
                if (!npcTargets.Contains(other.gameObject))
                {
                    npcTargets.Add(other.gameObject);
                }
            }
        }

        //removes anything exiting trigger from list
        private void OnTriggerExit(Collider other)
        {
            IEdible iEdible = other.gameObject.GetComponent<IEdible>();
            if (iEdible != null)
            {
                if (npcTargets.Contains(other.gameObject))
                {
                    npcTargets.Remove(other.gameObject);
                }
            }
        }
    }
}
