using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Maya
{


    public class Touch : MonoBehaviour
    {
        public bool isNearFood;
        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Food")
            {
                isNearFood = true;
            }
        }
    }
}
