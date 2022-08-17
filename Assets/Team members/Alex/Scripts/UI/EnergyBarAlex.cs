using System.Collections;
using System.Collections.Generic;
using John;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;

namespace Alex
{
    
    public class EnergyBarAlex : MonoBehaviour
    {
        public Energy myEnergy;
        public Slider slider;
        public Gradient gradient;
        public Image fill;

        public void SetMaxEnergy(float energyTest)
        {
            slider.maxValue = myEnergy.EnergyAmount.Value;
            slider.value = myEnergy.EnergyAmount.Value;

            fill.color = gradient.Evaluate(1f);
        }

        public void SetEnergy(float energyTest)
        {
            slider.value = myEnergy.EnergyAmount.Value;

            fill.color = gradient.Evaluate(slider.normalizedValue);
        }

        public void Update()
        {
            slider.value = myEnergy.EnergyAmount.Value;
            fill.color = gradient.Evaluate(slider.normalizedValue);
        }
    }
    
    
}
