using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Minh
{
    public class Food : MonoBehaviour
    {
        public GameObject food;
        // Start is called before the first frame update
        void Start()
        {
            //subcribe to Health script
            //GetComponent<Health>().Collectfood += UnityEngine.PlayerLoop.FixedUpdate;

        }

        // Update is called once per frame
        public void OnCollisionEnter(Collision collision)
        {
            Destroy(gameObject);
        }
    }
}