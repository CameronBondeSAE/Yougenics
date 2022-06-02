using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnergyUICamTest : MonoBehaviour
{
    public Energy          energy;
    public TextMeshProUGUI textMeshProUGUI;

    void Start()
    {
        energy.EnergyUpdatedEvent += UpdateText;
        energy.NoEnergyEvent      += RunOut;
    }

    void RunOut()
    {
        textMeshProUGUI.color = Color.red;
    }

    void UpdateText()
    {
        textMeshProUGUI.text = energy.energyAmount.ToString();
    }
}
