using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Maya
{



    public class Food : MonoBehaviour, IEdible, IItem
    {
        public int energyValue;
        public int scaleFactor;
        public int maxScale = 5;

        public ItemInfo info;
        
        void Awake()
        {
            scaleFactor = Random.Range(1, maxScale);
            gameObject.transform.localScale = new Vector3(scaleFactor, scaleFactor, scaleFactor);
            
        }

        public float GetEnergyAmount()
        {
            energyValue = scaleFactor * 10;
            return energyValue;
        }

        public float EatMe(float energyRemoved)
        {
            gameObject.transform.localScale = new Vector3(scaleFactor - energyRemoved, scaleFactor - energyRemoved,
                scaleFactor - energyRemoved);
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
