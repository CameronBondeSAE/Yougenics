using System.Collections;
using System.Collections.Generic;
using Ollie;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace K
{
    public class GameManager : MonoBehaviour
    {
        public float energyInsideDropOff;
        public EnergyContainer energyContainer;
        public TextMeshProUGUI energyUI;
        void OnEnable()
        {
            energyContainer.OnEnergyUpdate += UpdateEnergy;
        }
        
        void OnDisable()
        {
            energyContainer.OnEnergyUpdate -= UpdateEnergy;
        }

        private void UpdateEnergy()
        {
            Debug.Log("Event Triggered!");
            //just a place holder until confirmation with Cam.
            //will the player be able to drop off energy if they are not full yet
            //is there a currentEnergy variable I can use here if the above is not the case.
            
            //update GameManager energy
            //energyInsideDropOff += energyContainer.energy.energyMax;
            
            //update UI 
            //energyUI.SetText(energyInsideDropOff.ToString());
        }
    }

}
