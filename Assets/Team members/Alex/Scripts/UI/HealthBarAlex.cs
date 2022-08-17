using System.Collections;
using System.Collections.Generic;
using Minh;
using UnityEngine;
using UnityEngine.UI;

namespace Alex
{
    
    public class HealthBarAlex : MonoBehaviour
    {
        public Health myHealth;
        public Slider slider;
        public Gradient gradient;
        public Image fill;

        public void SetMaxHealth(float healthHp)
        {
            slider.maxValue = myHealth.CurrentHealth.Value;
            slider.value = myHealth.CurrentHealth.Value;

            fill.color = gradient.Evaluate(1f);
        }

        public void SetHealth(float healthHp)
        {
            slider.value = myHealth.CurrentHealth.Value;

            fill.color = gradient.Evaluate(slider.normalizedValue);
        }

        public void Update()
        {
            slider.value = myHealth.CurrentHealth.Value;
            fill.color = gradient.Evaluate(slider.normalizedValue);
        }
    }
}
