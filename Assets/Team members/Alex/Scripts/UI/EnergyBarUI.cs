using System.Collections;
using System.Collections.Generic;
using Minh;
using UnityEngine;
using UnityEngine.UI;


public class EnergyBarUI : MonoBehaviour
{
    public Energy myEnergy;
    public Slider slider;
    public Gradient gradient;
    public Image fill;
    private bool isVisible;

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
