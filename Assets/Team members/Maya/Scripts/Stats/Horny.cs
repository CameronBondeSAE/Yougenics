using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Maya
{
    public class Horny : MonoBehaviour
    {
        public float currentHorny;
        public float maxHorny = 100;
        public float riseSpeed;
        public float risingFactor;

        private void Update()
        {
            GettingHorny();
        }

        private IEnumerator GettingHorny()
        {
            while (currentHorny <= maxHorny)
            {
                yield return new WaitForSeconds(riseSpeed);
                {
                    currentHorny += risingFactor;
                }
            }
        }
    }
}
