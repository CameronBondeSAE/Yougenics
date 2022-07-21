using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.Netcode;


public class Energy : NetworkBehaviour
{
    //[SerializeField]
    public NetworkVariable<float> EnergyAmount = new NetworkVariable<float>();

    public float energyAmount = 50;
    public float energyMax = 100;
    public float energyMin = 0f;
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

    public void ChangeEnergy(float f)
    {
        if (IsOwner)
            EnergyAmount.Value += f;

        //energyAmount = energyAmount + f;
        CheckEnergyMax();
        CheckEnergyMin();
    }

    public void CheckEnergyMax()
    {
        if (EnergyAmount.Value >= energyMax)
        {
            if(IsOwner)
            {
                EnergyAmount.Value = energyMax;
                FullEnergyEvent?.Invoke();
                Debug.Log("Full Energy");
                FindObjectOfType<AudioManager>().Play("Energy Full");
            }
        }
    }

    public void CheckEnergyMin()
    {
        if (EnergyAmount.Value <= energyMin)
        {
            if(IsOwner)
            {
                EnergyAmount.Value = energyMin;

                NoEnergyEvent?.Invoke();
                Debug.Log("No Energy");
            }            
        }
        //Debug.Log("Editor test");
    }
    
    public IEnumerator EnergyDrainer()
    {
        while (EnergyAmount.Value >= energyMin)
        {
            yield return new WaitForSeconds(drainSpeed);
            {
                if(IsOwner)
                    EnergyAmount.Value -= drainAmount;
            }
        }
    }
}
