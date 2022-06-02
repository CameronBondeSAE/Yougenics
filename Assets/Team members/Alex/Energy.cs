using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Energy : MonoBehaviour
{

    public float energyAmount = 50;
    public float energyMax = 100;
    public float energyMin = 0;
    public float drainSpeed = 1;

    Health Hp; 

    // Start is called before the first frame update
    void Start()
    {
        CheckEnergyMax();

    }

    // Update is called once per frame
    void Update()
    {
        
        EnergyDrain();
        CheckEnergyMax();
        CheckEnergyMin();

    }

    public void CheckEnergyMax()
    {
        if (energyAmount == energyMax)
        {
            print("You have max energy");
        }
    }
    
    public void CheckEnergyMin()
    {
        if (energyAmount >= energyMin)
        {
            print("You are out of energy");
            //Hp = Hp -= 1;
        }
    }

    void EnergyDrain()
    {
        energyAmount = energyAmount - drainSpeed;
           
    }
    

}
