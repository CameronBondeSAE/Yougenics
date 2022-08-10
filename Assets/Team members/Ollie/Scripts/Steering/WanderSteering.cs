using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ollie
{
    public class WanderSteering : MonoBehaviour
    {
        private Rigidbody rigidbody;
        public float timer;
        public float rng;
        
        void Start()
        {
            rigidbody = GetComponent<Rigidbody>();
            rng = 0;
        }
        
        void Update()
        {
            if (timer > 2f)
            {
                timer = 0;
                rng = Random.Range(-15, 15);
                WanderTurn();
            }
            timer += Time.deltaTime;
        }
        
        void WanderTurn()
        {
            rigidbody.AddRelativeTorque(0,rng,0);
        }
    }
}
