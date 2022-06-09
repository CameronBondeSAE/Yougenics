using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Alex
{
    public class Sleeping : StateBase
    {

        public float regenEnergySpeed = 5f;

        private void OnEnable()
        {
          
            GetComponent<Renderer>().material.color = Color.white;
            
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
