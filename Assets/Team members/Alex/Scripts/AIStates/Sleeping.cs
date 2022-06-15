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
        

        private void OnEnable()
        {
            rb = GetComponent<Rigidbody>();
            GetComponent<Renderer>().material.color = Color.white;
            rb.velocity = Vector3.zero;
            
            // Lie down
            transform.DORotate(new Vector3(0, 0, 90f), 2f);
        }

        private void OnDisable()
        {
            transform.DORotate(new Vector3(0, 0, 0f), 0.6f);
        }

        private void Update()
        {
            GetComponent<Energy>().energyAmount += regenEnergySpeed * Time.deltaTime;
        }

    }
}
