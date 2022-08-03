using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace Cam
{


    public class SleepingState : StateBase
    {
        public float regenEnergySpeed = 4f;

        private void OnEnable()
        {
            // Lie down
            transform.DORotate(new Vector3(0, 0, 90f), 2f);
        }

        private void OnDisable()
        {
            transform.DORotate(new Vector3(0, 0, 0f), 0.6f);
        }

        // Update is called once per frame
        void Update()
        {
            GetComponent<Energy>().EnergyAmount.Value += regenEnergySpeed * Time.deltaTime;
        }
    }

}