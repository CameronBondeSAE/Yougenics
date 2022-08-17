using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ollie
{
    public class StateViewer : MonoBehaviour
    {
        private ParticleSystemRenderer particleSystem;
        public List<Material> particleMaterials;
        private CritterAIPlanner parent;

        private void Start()
        {
            particleSystem = GetComponent<ParticleSystemRenderer>();
            parent = GetComponentInParent<CritterAIPlanner>();
        }
    }
}
