using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Alex
{
    public class Food : MonoBehaviour, IEdible
    {
        public float totalFoodAmount = 50f;
        public float biteValue = 10f;
        public float currentFoodAmount;

        
        void Start()
        {
            GetComponent<Minh.Health>();
            //foodEnergy = GetComponent<Energy>();
            currentFoodAmount = totalFoodAmount;
        }

        private void OnTriggerEnter(Collider other)
        {
            //Restores energy to any object that has energy component on collison
            if (other.GetComponent<Energy>() != null)// ‘Fire’ the event
            {
                //GetComponent<Minh.Health>().Hp += 35;
                other.GetComponent<Energy>().EnergyAmount.Value += biteValue;
                FindObjectOfType<AudioManager>().Play("Energy Gain");
                //Destroy(gameObject);
                //print("Cheer");
            }
        }

        
        public float GetEnergyAmount()
        {
            return currentFoodAmount;
        }

        public float EatMe(float energyRemoved)
        {
            if (currentFoodAmount >= 0)
            {
                currentFoodAmount -= biteValue;
                energyRemoved = biteValue;
                
            }
            else
            {
                Destroy(gameObject);
            }

            return energyRemoved;
        }
        
    }

}
