using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Kevin
{
    public class GameManager : MonoBehaviour
    {
        //Energy variables 
        public float energyGoal;
        public Energy energy;
        
        //reference to Drop off point script 
        public DropOffPoint dropOffPoint;
        

        public delegate void EnergyGoal();
        public event EnergyGoal OnGoal;

        public void Start()
        {
            energyGoal = energy.energyMax;
        }

        public void OnEnable()
        {
            dropOffPoint.UpEnergy += UpdateEnergy; 
        }

        public void OnDisable()
        {
            dropOffPoint.UpEnergy -= UpdateEnergy; 
        }
        
        public void UpdateEnergy()
        {
            // HACK TODO actually take off energy from container
            energy.ChangeEnergy(50);
        }
    }
}

