using System.Collections;
using System.Collections.Generic;
using John;
using Minh;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;
using UnityEngine.ProBuilder.MeshOperations;

namespace Alex
{
    
    public class EnergyBarAlex : MonoBehaviour
    {
        public Energy myEnergy;
        public Slider slider;
        public Gradient gradient;
        public Image fill;
        ItemSlot itemSlot;
        public bool isItem1;
        public bool isItem2;

        public void Start()
        {
            itemSlot = GetComponentInParent<ItemSlot>();
            itemSlot.item1PickedUpEvent += UpdateItem1Active;
            itemSlot.item2PickedUpEvent += UpdateItem2Active;
            
            if (isItem1)
                gameObject.SetActive(false);
            if (isItem2)
                gameObject.SetActive(false);
        }

        public void SetMaxEnergy(float energyTest)
        {
            
            gameObject.SetActive(true);
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

        public void UpdateItem1Active(bool isActive)
        {
            if(isItem1)
            if (isActive)
                gameObject.SetActive(true);

            else if (isItem1)
                gameObject.SetActive(false);
        }
        
        public void UpdateItem2Active(bool isActive)
        {
            if(isItem2)
            if (isActive)
                gameObject.SetActive(true);

            else if (isItem2)
                gameObject.SetActive(false);
        }
    }
    
    
}
