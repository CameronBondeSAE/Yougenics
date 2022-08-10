using System.Collections;
using System.Collections.Generic;
using Minh;
using UnityEngine;
using UnityEngine.UI;
public class HpUI : MonoBehaviour
{
    public Health health;
    public Slider slider;
    public void SetMaxHealth(int health)
    {
        slider.maxValue = health;
        slider.value = health;
    }

    public void SetHealth(int health)
    {
        slider.value = health;
    }
}
