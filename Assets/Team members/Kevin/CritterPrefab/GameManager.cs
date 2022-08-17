using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using Ollie;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

namespace Kevin
{
    
    public class GameManager : MonoBehaviour
    {
        public static GameManager instance;
        //Energy variables 
        public float energyGoal;
        public Energy energy;
        public float currentEnergy;
        public float remainingTime;
        
        
        //reference to Drop off point script 
        //public DropOffPoint dropOffPoint;
        

        public delegate void EnergyGoal();
        public event EnergyGoal OnGoal;
        public List<GameObject> dropOffPoints;
        private bool currentlyDraining;
        public float drainRate;
        

        public void Awake()
        {
            instance = this;
            energy = GetComponent<Energy>();
            energyGoal = energy.energyMax;
            
            dropOffPoints = new List<GameObject>();
            StartCoroutine(DrainCoroutine());
        }

        public void Update()
        {
            currentEnergy = energy.EnergyAmount.Value;

            if (remainingTime > 0)
            {
                remainingTime -= Time.deltaTime;
            }
        }
        public IEnumerator DrainCoroutine()
        {
            currentlyDraining = true;
            if (dropOffPoints.Count > 0) // also need to turn this off when container's energy is full
            {
                foreach (GameObject target in dropOffPoints)
                {
                    if (target.GetComponent<DropOffPoint>().energy.EnergyAmount.Value > 0)
                    {
                        target.GetComponent<DropOffPoint>().energy.ChangeEnergy(-drainRate);
                        energy.ChangeEnergy(drainRate);
                        //print("Energy being received in drop off point");
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

        public void UpdateDropOffPoints()
        {
            if (dropOffPoints != null && !dropOffPoints.Contains(GetComponent<DropOffPoint>().gameObject))
                dropOffPoints.Add(GetComponent<DropOffPoint>().gameObject);
        }
    }
}

