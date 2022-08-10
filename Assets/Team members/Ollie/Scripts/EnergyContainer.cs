using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Ollie
{
    public class EnergyContainer : MonoBehaviour, IItem, IEnergyDrainer
    {
        Energy energy;
        public float drainRate;
        private bool currentlyDraining;
        public List<GameObject> drainTargets;
        
        private void Awake()
        {
            energy = GetComponent<Energy>();
            drainTargets = new List<GameObject>();
            currentlyDraining = false;
            StartCoroutine(DrainCoroutine());
        }

        public IEnumerator DrainCoroutine()
        {
            currentlyDraining = true;
            if (drainTargets.Count > 0) // also need to turn this off when container's energy is full
            {
                foreach (GameObject target in drainTargets)
                {
                    if (target.GetComponent<Energy>().EnergyAmount.Value > 0)
                    {
                        //energy.ChangeEnergy(target.GetComponent<Energy>().ChangeEnergy(-drainRate));
                        target.GetComponent<Energy>().ChangeEnergy(-drainRate);
                        energy.ChangeEnergy(drainRate);
                        print("yoink");
                    }
                    else if  (target.GetComponent<Energy>().EnergyAmount.Value <= 0)
                    {
                        currentlyDraining = false;
                    }
                }
            }
            yield return new WaitForSeconds(1f);
            StartCoroutine(DrainCoroutine());
            currentlyDraining = false;
        }

        public void OnTriggerEnter(Collider other)
        {
            //need to check for player too!! maybe?
            if (other.GetComponent<Energy>() != null && !drainTargets.Contains(other.gameObject))
            {
                drainTargets.Add(other.gameObject);
                if(other.GetComponent<DropOffPoint>() != null)
                drainTargets.Remove(other.GetComponent<DropOffPoint>().gameObject);
            }
        }

        public void OnTriggerExit(Collider other)
        {
            if (drainTargets.Contains(other.gameObject))
            {
                drainTargets.Remove(other.gameObject);
            }
        }

        #region IItem Interface

        public void SpawnedAsNormal()
        {

        }

        public ItemInfo GetInfo()
        {
            throw new NotImplementedException();
        }

        public object transform { get; set; }

        #endregion
    }
}