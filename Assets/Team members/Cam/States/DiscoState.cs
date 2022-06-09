using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;


namespace Cam
{


    public class DiscoState : StateBase
    {
        private void OnEnable()
        {
            // POW!
            float enablePunchSize = 1.2f;
            transform.DOPunchScale(new Vector3(enablePunchSize, enablePunchSize, enablePunchSize), 0.5f);
        }

        private void OnDisable()
        {
            // Back to normal
            GetComponent<Renderer>().material.color = Color.white;
        }


        // Update is called once per frame
        void Update()
        {
            // Flash colours
            GetComponent<Renderer>().material.color = new Color(Random.value, Random.value, Random.value);
        }
    }

}