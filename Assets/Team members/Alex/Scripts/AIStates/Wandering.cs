using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Alex
{
    public class Wandering : StateBase
    {
        Rigidbody rb;
        public float movementSpeed = 1f;

        public void OnEnable()
        {
            //Changes color to yellow when entering this state
            rb = GetComponent<Rigidbody>();
            GetComponent<Renderer>().material.color = Color.yellow;
        }
        
        public void FixedUpdate()
        {
            //Stops them from always moving 
            if (Random.Range(0, 100) == 0)
                WonderAround();
        }
        private void WonderAround()
        {
            //Move to a new location within the range given, low values used so they dont go to the other side of the terrain
            rb.velocity = new Vector3(Random.Range(-5, 5), 0, Random.Range(-5, 5));
        }
    }
}