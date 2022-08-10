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
        void Start()
        {
            
            rb = GetComponent<Rigidbody>(); 
        }
        void Update()
        {
            perlinValue = Mathf.PerlinNoise(Random.Range(minAngle,maxAngle), Random.Range(minAngle,maxAngle));
            
            rb.AddRelativeTorque(0, Random.Range(minAngle,maxAngle) * perlinValue * Time.deltaTime * Random.Range(-1,1) * Time.deltaTime, 0);
        }
        
    }
}

