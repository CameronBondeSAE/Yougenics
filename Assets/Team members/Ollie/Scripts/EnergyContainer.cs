using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ollie
{
    public class EnergyContainer : MonoBehaviour, IItem
    {
        public Energy energy;

        public float drainRate;
        private bool currentlyDraining;

        public List<GameObject> drainTargets;
        

        #region Kevin Testing GameManager
        
        public delegate void EnergyDropOff();
        public event EnergyDropOff OnEnergyUpdate;

        public void EnergyUpdate()
        {
            OnEnergyUpdate?.Invoke();
        }
        

        #endregion
        private void Awake()
        {
            energy = GetComponent<Energy>();
            drainTargets = new List<GameObject>();
            currentlyDraining = false;
            StartCoroutine(DrainCoroutine());
        }

        private void Update()
        {
        }

        IEnumerator DrainCoroutine()
        {
            currentlyDraining = true;
            if (drainTargets.Count > 0) // also need to turn this off when container's energy is full
            {
                foreach (GameObject target in drainTargets)
                {
                    energy.ChangeEnergy(target.GetComponent<Energy>().ChangeEnergy(-drainRate));
                    print("yoink");
                }
            }

            yield return new WaitForSeconds(1f);
            StartCoroutine(DrainCoroutine());
            currentlyDraining = false;
        }

        private void OnTriggerEnter(Collider other)
        {
            //need to check for player too!! maybe?
            if (other.GetComponent<Energy>() != null && !drainTargets.Contains(other.gameObject))
            {
                drainTargets.Add(other.gameObject);
            }
        }

        private void OnTriggerExit(Collider other)
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