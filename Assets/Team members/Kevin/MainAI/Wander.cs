using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kevin
{
    public class Wander : MonoBehaviour
    {
        private Rigidbody rb;
        [SerializeField] private float perlinValue;
        [SerializeField] private float minAngle;
        [SerializeField] private float maxAngle;
        public float oneTick;
        public float tickRotation; 
        void Start()
        {
            
            rb = GetComponent<Rigidbody>(); 
        }
        void Update()
        {
            if (oneTick > tickRotation)
            {
                oneTick = 0f;
                perlinValue = (Mathf.PerlinNoise(rb.transform.position.x, rb.transform.position.z))*2-1;
                Patrol();
            }

            oneTick += Time.deltaTime;

        }

        void Patrol()
        {
            rb.AddRelativeTorque(0, perlinValue * 5f, 0,ForceMode.Impulse);
        }
        
    }
}

