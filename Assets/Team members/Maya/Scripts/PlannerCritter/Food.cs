using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Maya
{



    public class Food : MonoBehaviour
    {
        public int energyValue;
        public int scaleFactor;
        public int maxScale = 5;
        void Awake()
        {
            scaleFactor = Random.Range(1, maxScale);
            this.gameObject.transform.localScale = new Vector3(scaleFactor, scaleFactor, scaleFactor);
            energyValue = scaleFactor * 10;
        }
    }
}
