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
            GetComponent<Health>().Collectfood += FixedUpdate;

        }

        // Update is called once per frame
        private void FixedUpdate()
        {

        }

    }
}