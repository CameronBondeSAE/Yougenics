using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Maya
{



    public class Food : MonoBehaviour
    {
        public float energyValue;
        public float scaleFactor;
        public int maxScale = 5;

        // Start is called before the first frame update
        void Awake()
        {
            scaleFactor = Random.Range(1, maxScale);
            this.gameObject.transform.localScale = new Vector3(scaleFactor, scaleFactor, scaleFactor);
            energyValue = scaleFactor * 10;
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
