using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace Alex
{
    public class Sleeping : StateBase
    {
        public Rigidbody rb;
        public float regenEnergySpeed = 2f;
        
        public void OnEnable()
        {
            //Changes the colour to white, and stops them from moving
            rb = GetComponent<Rigidbody>();
            GetComponent<Renderer>().material.color = Color.white;
            rb.velocity = Vector3.zero;
            
            // Makes the AI lay down 
            transform.DORotate(new Vector3(0, 0, 90f), 2f);
        }

        private void OnDisable()
        {
            //Stands the object up once they leave the state
            transform.DORotate(new Vector3(0, 0, 0f), 0.6f);
        }

        private void Update()
        {
            //While asleep they gain energy
            GetComponent<Energy>().energyAmount += regenEnergySpeed * Time.deltaTime;
        }

    }
}
