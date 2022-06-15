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

    void NoEnergy()
    {
        print("You have no energy");
        
    }

    void FullEnergy()
    {
        print("You have max energy");
    }

    // Update is called once per frame
    void Update()
    {
        
        //EnergyDrain();
        CheckEnergyMax();
        CheckEnergyMin();
        EnergyDrainer();

    }

    void CheckEnergyMax()
    {
        if (energyAmount >= energyMax)
        {
            energyAmount = energyMax ; 
            FullEnergyEvent?.Invoke();
        }
    }
    
    void CheckEnergyMin()
    {
        if (energyAmount <= energyMin)
        {
            energyAmount = energyMin;
            
            NoEnergyEvent?.Invoke();
                        
        }
    }

    /*
    void EnergyDrain()
    {

        energyAmount = energyAmount - drainSpeed;
           
    }
    */

    private IEnumerator EnergyDrainer()
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
