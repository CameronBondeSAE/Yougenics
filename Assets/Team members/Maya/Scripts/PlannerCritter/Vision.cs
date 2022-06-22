using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Maya
{


    public class Vision : MonoBehaviour
    {
        public List<Food> foodIveSeen;
        public List<Vector3> foodBank;

        private void OnTriggerEnter(Collider other)
        {
            Food onePiece = other.gameObject.GetComponent<Food>();
            if (other.tag == "Food" && !foodIveSeen.Contains(onePiece))
            {
                foodIveSeen.Add(onePiece);
            }
        }
    }
}
