using Alex;
using Kevin;
using UnityEngine;
using TMPro;

public class TotalEnergyGathered : MonoBehaviour
{
    TMP_Text textMeshPro;
    public GameObject energyBase;
    public GameManager gameManager;
    Energy energy;

    float totalEnergy;
    
    
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GetComponent<GameManager>();
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
        //totalEnergy = gameManager.GetComponent<Energy>().ChangeEnergy();
    }
}
