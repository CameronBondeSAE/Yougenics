using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Maya
{


    public class Touch : MonoBehaviour
    {
        public Food foodImTouching;
        public float timeToEat;
        public bool isNearFood;
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Food"))
            {
                foodImTouching = other.GetComponent<Food>();
                timeToEat = foodImTouching.energyValue / 2;
                isNearFood = true;
            }
        }
    }
}
