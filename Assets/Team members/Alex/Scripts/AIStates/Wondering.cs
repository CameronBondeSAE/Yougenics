using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Alex
{
    public class Wondering : StateBase
    {
        Rigidbody rb;
        public float movementSpeed = 1f;

        public void OnEnable()
        {
            rb = GetComponent<Rigidbody>();
            //rb.velocity = new Vector3(Random.Range(-1,1), 0, Random.Range(-1,1));
        }

        public void OnDisable()
        {
        }

        public void FixedUpdate()
        {
            if (Random.Range(0, 100) == 0)
                WonderAround();
        }


        private void WonderAround()
        {
            rb.velocity = new Vector3(Random.Range(-5, 5), 0, Random.Range(-5, 5));
            Debug.Log("Wondering");
        }
    }
}