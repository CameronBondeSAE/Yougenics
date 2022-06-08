using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System; 

public class Energy : MonoBehaviour
{

    public float energyAmount = 50;
    public float energyMax = 100;
    public float energyMin = 0;
    public float drainSpeed = 1;
    public event Action NoEnergyEvent;
    public event Action FullEnergyEvent;

    

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Health>();
        CheckEnergyMax();

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
        
        EnergyDrain();
        CheckEnergyMax();
        CheckEnergyMin();

    }

    void CheckEnergyMax()
    {
        if (energyAmount >= energyMax)
        {
            FullEnergyEvent?.Invoke();
        }
    }
    
    void CheckEnergyMin()
    {
        if (energyAmount <= energyMin)
        {

            NoEnergyEvent?.Invoke();
            //Hp = Hp -= 1;
        }
    }

    void EnergyDrain()
    {
        energyAmount = energyAmount - drainSpeed;
           
    }
    

}
