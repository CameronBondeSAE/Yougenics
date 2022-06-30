using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System; 

public class Energy : MonoBehaviour
{

    public float energyAmount = 50;
    public float energyMax = 100;
    public float energyMin = 0;
    public float drainAmount = 1;
    public event Action NoEnergyEvent; 
    public event Action FullEnergyEvent;
    public float drainSpeed = 1;
    

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Minh.Health>();
        CheckEnergyMax();
        StartCoroutine(EnergyDrainer());

    }

    void Update()
    {
        //EnergyDrain();
        CheckEnergyMax();
        CheckEnergyMin();
        EnergyDrainer();
    }

    public void CheckEnergyMax()
    {
        if (energyAmount >= energyMax)
        {
            energyAmount = energyMax ; 
            FullEnergyEvent?.Invoke();
            Debug.Log("Full Energy");
        }
    }

    public void CheckEnergyMin()
    {
        if (energyAmount <= energyMin)
        {
            energyAmount = energyMin;
            
            NoEnergyEvent?.Invoke();
            Debug.Log("No Energy");
                        
        }
        //Debug.Log("Editor test");
    }
    
    public IEnumerator EnergyDrainer()
    {
        while (energyAmount >= energyMin)
        {
            yield return new WaitForSeconds(drainSpeed);
            {
                energyAmount -= drainAmount;
            }
        }
    }   
}
