using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Alex
{

    public class Food : MonoBehaviour
    {
        
        public float myFoodAmount = 10f;
        Energy foodEnergy;

        // Start is called before the first frame update
        void Start()
        {
            
            GetComponent<Minh.Health>();
            //foodEnergy = GetComponent<Energy>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<Energy>() != null)// ‘Fire’ the event
            {
                GetComponent<Minh.Health>().Hp += 35;
                other.GetComponent<Energy>().energyAmount += myFoodAmount;
                Destroy(gameObject);
                //print("Cheer");
            }

        }

    }

}
