using System;
using System.Collections;
using System.Collections.Generic;
using Kevin;
using Ollie;
using Sirenix.OdinInspector;
using UnityEngine;

public class DropOffPoint : MonoBehaviour
{
    public GameObject gameManagerObject;
    public GameManager gameManager;

    //update energy event to occur after triggering the box
    public delegate void UpdateEnergy();
    public event UpdateEnergy UpEnergy; 
    
    public void Start()
    {
        gameManager = gameManagerObject.GetComponent<GameManager>();
    }
    public void OnTriggerEnter(Collider other)
    {
        EnergyContainer otherEnergy = other.GetComponent<EnergyContainer>();
        if (otherEnergy != null)
        {
            //updates the current energy amount in the base by adding the other player's energy container amount.
            //gameManager.currentEnergyAmount += otherEnergy.energy;
            
            //fires the event in game manager
            UpEnergy?.Invoke();
        }
    }
}
