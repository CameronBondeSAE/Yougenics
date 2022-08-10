using System;
using System.Collections;
using System.Collections.Generic;
using Kevin;
using Ollie;
using Sirenix.OdinInspector;
using UnityEngine;

[SelectionBase]
public class DropOffPoint : MonoBehaviour, IEnergyDrainer
{
    
    public Energy energy;
    public Energy gameManagerEnergy;
    private bool currentlyDraining;
    public List<GameObject> energyContainers;
    public float drainRate;
    public GameManager gameManager;

    void Awake()
    {
        GameManager.instance.dropOffPoints.Add(gameObject);
        //energy = GetComponent<Energy>();
        energyContainers = new List<GameObject>();
        StartCoroutine(DrainCoroutine());
    }
    
    
    // HACK change from stay to a corouting or something
    public void OnTriggerStay(Collider other)
    {
        /*
        EnergyContainer otherEnergy = other.GetComponent<EnergyContainer>();
        if (otherEnergy != null)
        {
            DrainFromContainer();
            // TODO remove energy from container gradually

            // TODO put "energy container" energy into our energy component

        }
        
        if (other.GetComponent<EnergyContainer>() != null && !drainTargets.Contains(other.gameObject))
        {
            drainTargets.Add(other.gameObject);
        }
        */
    }


    public IEnumerator DrainCoroutine()
    {
        currentlyDraining = true;
        if (energyContainers.Count > 0) // also need to turn this off when container's energy is full
        {
            foreach (GameObject target in energyContainers)
            {
                if (target.GetComponent<Energy>().EnergyAmount.Value > 0)
                {
                    target.GetComponent<Energy>().ChangeEnergy(-drainRate);
                    //gameManager.energy.ChangeEnergy(drainRate);
                    energy.ChangeEnergy(drainRate);
                    //print("Energy being received in drop off point");
                }
                
                else if  (target.GetComponent<Energy>().EnergyAmount.Value <= 0)
                {
                    currentlyDraining = false;
                }
            }
            
        }
        yield return new WaitForSeconds(1f);
        StartCoroutine(DrainCoroutine());
        currentlyDraining = false;
    }
    
    public void OnTriggerEnter(Collider other)
    {
        //need to check for player too!! maybe?
        if (other.GetComponent<EnergyContainer>() != null && !energyContainers.Contains(other.gameObject))
        {
            energyContainers.Add(other.gameObject);
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (energyContainers.Contains(other.gameObject))
        {
            energyContainers.Remove(other.gameObject);
        }
    }
    void FixedUpdate()
    {
        //GetComponent<GameManager>().energy.EnergyAmount.Value += energy.EnergyAmount.Value;

        //gameManagerEnergy.EnergyAmount.Value += energy.EnergyAmount.Value;
    }
}
