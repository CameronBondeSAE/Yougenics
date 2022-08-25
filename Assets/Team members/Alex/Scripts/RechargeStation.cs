using System;
using System.Collections;
using System.Collections.Generic;
using Kevin;
using Ollie;
using Sirenix.OdinInspector;
using UnityEngine;

[SelectionBase]
public class RechargeStation : MonoBehaviour, IEnergyDrainer
{
    
    public Energy energy;
    private bool currentlyCharging;
    public List<GameObject> energyUsers;
    public float rechargeAmount;

    public Renderer cube;
    public Material inactive;
    public Material active;
    

    void Awake()
    {
        //energy = GetComponent<Energy>();
        energyUsers = new List<GameObject>();
        StartCoroutine(DrainCoroutine());
    }

    void Start()
    {
        
    }

    public IEnumerator DrainCoroutine()
    {
        currentlyCharging = true;
        if (energyUsers.Count > 0) // also need to turn this off when container's energy is full
        {
            foreach (GameObject target in energyUsers)
            {
                if (target.GetComponent<Energy>().EnergyAmount.Value >= 0)
                {
                    target.GetComponent<Energy>().ChangeEnergy(rechargeAmount);
                    GameManager.instance.energy.ChangeEnergy(-target.GetComponent<Energy>().ChangeEnergy(rechargeAmount));

                    cube.material = active;
                }
                
                else if  (target.GetComponent<Energy>().EnergyAmount.Value <= 0)
                {
                    currentlyCharging = false;
                    cube.material = inactive;
                }
            }
            
        }
        else
        {
            currentlyCharging = false;
            cube.material = inactive;
        }
        yield return new WaitForSeconds(1f);
        StartCoroutine(DrainCoroutine());
        currentlyCharging = false;
    }
    
    public void OnTriggerEnter(Collider other)
    {
        //need to check for player too!! maybe?
        if (other.GetComponent<Energy>() != null && !energyUsers.Contains(other.gameObject) && !other.GetComponent<JohnPlayerModel>())
        {
            energyUsers.Add(other.gameObject);
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (energyUsers.Contains(other.gameObject))
        {
            energyUsers.Remove(other.gameObject);
        }
    }

    public void SpawnedAsNormal()
    {
        throw new NotImplementedException();
    }
}
