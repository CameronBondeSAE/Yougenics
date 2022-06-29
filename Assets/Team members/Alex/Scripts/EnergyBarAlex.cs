using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Alex
{
    
    public class EnergyBarAlex : MonoBehaviour
    {
        public Energy energy;
        public Slider slider;
        public Gradient gradient;
        public Image fill;

        public void SetMaxEnergy(float energyTest)
        {
            slider.maxValue = energy.energyAmount;
            slider.value = energy.energyAmount;

            fill.color = gradient.Evaluate(1f);
        }

        public void SetEnergy(float energyTest)
        {
            slider.value = energy.energyAmount;

            fill.color = gradient.Evaluate(slider.normalizedValue);
        }

        public void Update()
        {
            slider.value = energy.energyAmount;
            fill.color = gradient.Evaluate(slider.normalizedValue);
        }
    }
}
