using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Maya
{


    public class Vision : MonoBehaviour
    {
        public List<Food> foodIveSeen;

        private void OnTriggerEnter(Collider other)
        {
            Food onePiece = other.gameObject.GetComponent<Food>();
            if (other.CompareTag("Food") && !foodIveSeen.Contains(onePiece))
            {
                foodIveSeen.Add(onePiece);
            }
        }
    }
}
