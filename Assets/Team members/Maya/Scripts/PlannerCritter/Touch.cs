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

        public Horny mateImTouching;
        public bool myMatesHornyToo;
        public float timeToMate;
        public bool isNearMate;
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Food"))
            {
                foodImTouching = other.GetComponent<Food>();
                timeToEat = foodImTouching.energyValue / 2;
                isNearFood = true;
            }
            
            if (other.CompareTag("MayasCritter"))
            {
                mateImTouching = other.GetComponent<Horny>();
                if(mateImTouching.currentHorny >= 75)
                {
                    myMatesHornyToo = true;
                }
                timeToMate = mateImTouching.currentHorny / 3;
                isNearMate = true;
                
            }
        }
    }
}
