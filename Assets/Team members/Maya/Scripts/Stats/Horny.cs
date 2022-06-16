using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Maya
{
    public class Horny : MonoBehaviour
    {
        public float currentHorny;
        public float minHorny;
        public float maxHorny = 100;
        public float drainSpeed;
        public float drainFactor;

        private void Update()
        {
            GettingHorny();
        }

        private IEnumerator GettingHorny()
        {
            while (currentHorny >= minHorny)
            {
                yield return new WaitForSeconds(drainSpeed);
                {
                    currentHorny -= drainFactor;
                }
            }
        }
    }
}
