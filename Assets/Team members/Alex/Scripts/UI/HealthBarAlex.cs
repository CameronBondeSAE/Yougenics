using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Alex
{
    
    public class HealthBarAlex : MonoBehaviour
    {
        public Minh.Health minhHealth;
        public Slider slider;
        public Gradient gradient;
        public Image fill;

        public void SetMaxHealth(float healthHp)
        {
            slider.maxValue = minhHealth.CurrentHealth.Value;
            slider.value = minhHealth.CurrentHealth.Value;

            fill.color = gradient.Evaluate(1f);
        }

        public void SetHealth(float healthHp)
        {
            slider.value = minhHealth.CurrentHealth.Value;

            fill.color = gradient.Evaluate(slider.normalizedValue);
        }

        public void Update()
        {
            slider.value = minhHealth.CurrentHealth.Value;
            fill.color = gradient.Evaluate(slider.normalizedValue);
        }
    }
}
