using System;
using System.Collections;
using System.Collections.Generic;
using Minh;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Minh
{
    public class Graphpnoise : MonoBehaviour
    {
        public GameObject Shakemiddlecube;
        public GameObject Wibbleleftcube;
        public GameObject Sinrightcube;
        public float adjust1;
        public float adjust2;
        public float adjust3;
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            Shakemiddlecube.transform.position = new Vector3(2, Random.Range(0,1f), 0);
            Wibbleleftcube.transform.position = new Vector3(0, Mathf.PerlinNoise(Time.time, 0), 0);
            Sinrightcube.transform.position = new Vector3(4, MathF.Sin(Time.time) * Mathf.Sin(Time.time* 1.1f), 0);

        }
    }
    // Start is called before the first frame update
   
}
