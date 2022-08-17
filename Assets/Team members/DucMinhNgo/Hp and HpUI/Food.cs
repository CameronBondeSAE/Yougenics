using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Minh
{
    public class Food : MonoBehaviour
    {
        public GameObject food;

        public Health health;
        // Start is called before the first frame update
        void Start()
        {
            //subcribe to Health script
            GetComponent<Health>().Food += Eatfood;

        }

        // Update is called once per frame
        public void OnCollisionEnter(Collision collision)
        {
            Destroy(gameObject);
            Instantiate(food, new Vector3( 0, Random.Range(0,3), 0), Quaternion.identity);
        }

        public void Eatfood()
        {
            
        }
    }
}