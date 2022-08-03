using Alex;
using UnityEngine;
using TMPro;

public class TotalEnergyGathered : MonoBehaviour
{
    TMP_Text textMeshPro;
    public GameObject energyBase;

    float totalEnergy;
    
    
    // Start is called before the first frame update
    void Start()
    {
        textMeshPro = GetComponent<TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {
        EnergyLevel();
        textMeshPro.text = "Total Energy Gathered: " + totalEnergy.ToString("#"); 
    }

    public void EnergyLevel()
    {
        totalEnergy = energyBase.GetComponent<Energy>().EnergyAmount.Value;
    }
}
