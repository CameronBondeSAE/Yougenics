using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Maya
{



    [RequireComponent(typeof(Energy))]
    public class Food : MonoBehaviour, IEdible, IItem
    {
        public int energyValue;
        public int scaleFactor;
        public int maxScale = 5;
        Energy     energy;

        public ItemInfo info;
        
        void Awake()
        {
            scaleFactor = Random.Range(1, maxScale);
            gameObject.transform.localScale = new Vector3(scaleFactor, scaleFactor, scaleFactor);
            energy = GetComponent<Energy>();
        }

        public float GetEnergyAmount()
        {
            return energy.EnergyAmount.Value;
        }

        public float EatMe(float energyRemoved)
        {
            energy.EnergyAmount.Value -= energyRemoved;
            
            // View
            gameObject.transform.localScale = new Vector3(energy.EnergyAmount.Value * scaleFactor, energy.EnergyAmount.Value * scaleFactor,
                                                          energy.EnergyAmount.Value * scaleFactor);
            return energyRemoved;
        }

        public void     SpawnedAsNormal()
        {
            
        }

        public ItemInfo GetInfo()
        {
            return info;
        }
    }
}
