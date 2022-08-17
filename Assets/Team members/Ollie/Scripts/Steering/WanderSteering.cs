using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Ollie
{
    public class WanderSteering : MonoBehaviour
    {
        private Rigidbody rigidbody;
        private CritterAIPlanner parent;
        public float timer;
        public float timeToWanderRotation;
        [FormerlySerializedAs("rng")] public float perlinRNG;
        
        void Start()
        {
            rigidbody = GetComponentInParent<Rigidbody>();
            parent = GetComponentInParent<CritterAIPlanner>();
            perlinRNG = 0;
            timeToWanderRotation = 2;
        }
        
        void Update()
        {
            if (timer > timeToWanderRotation)
            {
                timer = 0;
                if (parent.moveSpeed > 0)
                {
                    perlinRNG = (Mathf.PerlinNoise(rigidbody.transform.position.x, rigidbody.transform.position.z))*2-1;
                    WanderTurn();
                }
            }
            timer += Time.deltaTime;
        }
        
        void WanderTurn()
        {
            rigidbody.AddRelativeTorque(0,perlinRNG,0,ForceMode.Impulse);
        }
    }
}
