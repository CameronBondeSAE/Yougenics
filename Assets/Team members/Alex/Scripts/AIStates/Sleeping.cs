using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Alex
{
    public class Sleeping : StateBase
    {
        public Rigidbody rb;
        public float regenEnergySpeed = 2f;
        

        private void OnEnable()
        {
            rb = GetComponent<Rigidbody>();
            GetComponent<Renderer>().material.color = Color.white;
            rb.velocity = Vector3.zero;
        }

        private void OnDisable()
        {
            
        }

        private void Update()
        {
            GetComponent<Energy>().energyAmount += regenEnergySpeed * Time.deltaTime;
        }

    }
}
