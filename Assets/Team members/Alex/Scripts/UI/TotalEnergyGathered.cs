using System.Collections.Generic;
using Alex;
using Kevin;
using UnityEngine;
using TMPro;

public class TotalEnergyGathered : MonoBehaviour
{
    private TMP_Text textMeshPro;

    // Start is called before the first frame update
    void Start()
    {
        textMeshPro = GetComponent<TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {
        //EnergyLevel();
        textMeshPro.text = "Total Energy Gathered:" + "\n\r" + GameManager.instance.GetComponent<Energy>().EnergyAmount.Value.ToString("#") + "/" + GameManager.instance.energyGoal; 
    }
}
