using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Alex
{

    public class Food : MonoBehaviour
    {
        public int myFoodAmount = 10;
        Energy foodEnergy;

        // Start is called before the first frame update
        void Start()
        {
            foodEnergy = GetComponent<Energy>();
        }

        // Update is called once per frame
        void Update()
        {


        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<Energy>() != null)// ‘Fire’ the event
            {
                other.GetComponent<Energy>().energyAmount += myFoodAmount;
                Destroy(gameObject);
                //print("Cheer");
            }

        }

    }

}
