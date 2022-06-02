using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kevin
{
    public class Fox : CritterBase
    {
        public Transform entityTransform;
        // Start is called before the first frame update
        void Start()
        {
            health = 100f;
            energy = 100f; 
        }
    } 
}

