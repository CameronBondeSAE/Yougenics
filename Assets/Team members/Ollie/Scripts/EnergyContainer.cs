using John;
using System;
using System.Collections;
using System.Collections.Generic;
using Anthill.Pool;
using UnityEngine;


namespace Ollie
{
    public class EnergyContainer : MonoBehaviour, IItem, IEnergyDrainer
    {
        public Energy energy;
        public float drainRate;
        private bool currentlyDraining;
        public List<GameObject> drainTargets;
        public ItemInfo iteminfo;
        //public GameObject orb;

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
                        //orb.SetActive(true);
                        
                    }
                    else if  (target.GetComponent<Energy>().EnergyAmount.Value <= 0)
                    {
                        currentlyDraining = false;
                        //orb.SetActive(false);
                    }
                }
            }
            yield return new WaitForSeconds(1f);
            StartCoroutine(DrainCoroutine());
            currentlyDraining = false;
        }

        public void OnTriggerEnter(Collider other)
        {
            IItem otherItem = other.GetComponent<IItem>();
            //need to check for player too!! maybe?
            if (other.GetComponent<Energy>() != null && !drainTargets.Contains(other.gameObject) && !other.GetComponent<PlayerModel>() && !other.GetComponent<DropOffPoint>() && otherItem != null)
            {
                drainTargets.Add(other.gameObject);
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
            return iteminfo;
        }

        public object transform { get; set; }

        #endregion
    }
}