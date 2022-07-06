using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Maya
{
    public class Rage : MonoBehaviour
    {
        public float currentRage;
        public float maxRage = 100;
        public float riseSpeed;
        public float risingFactor;


        private void Start()
        {
            StartCoroutine(GettingAngry());
        }

        private void Update()
        {
            GettingAngry();
        }

        private IEnumerator GettingAngry()
        {
            while (currentRage <= maxRage)
            {
                yield return new WaitForSeconds(riseSpeed);
                {
                    currentRage += risingFactor;
                }
            }
        }
    }
}