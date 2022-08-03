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
        public float currentEnergyAmount;
        
        //reference to Drop off point script 
        public DropOffPoint dropOffPoint;
        
        //reference to the UI/Text mesh gameobject
        public TMP_Text currentEnergyUI;

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
            Debug.Log("event occurred");
            //Update the UI text here
            currentEnergyUI.SetText(currentEnergyAmount.ToString());
        }
    }
}

