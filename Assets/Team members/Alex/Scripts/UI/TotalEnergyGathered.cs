using Alex;
using Kevin;
using UnityEngine;
using TMPro;

public class TotalEnergyGathered : MonoBehaviour
{
    private TMP_Text textMeshPro;
    public GameManager gameManager;
    Energy energy;

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
        textMeshPro.text = "Total Energy Gathered:" + "\n\r" + totalEnergy.ToString("#"); 
    }

    public void EnergyLevel()
    {
        totalEnergy = gameManager.GetComponent<Energy>().EnergyAmount.Value;
    }
}
