using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minh
{
    public class Basestate : MonoBehaviour
    {
        // Start is called before the first frame update
        private void Awake()
        {
            GetComponent<Renderer>().material.color = Color.red;
        }

        public Basestate()
        {
            
            Debug.Log("idle animation play");
        }


        // Update is called once per frame
        void Update()
        {
        
        }
    }
}

