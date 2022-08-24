using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Creature : CreatureBase, IEdible
{
    public float EatMe(float energyRemoved)
    {
        throw new System.NotImplementedException();
    }

    public float GetEnergyAmount()
    {
        throw new System.NotImplementedException();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(age >= maxAge)
        {

        }
    }
}
