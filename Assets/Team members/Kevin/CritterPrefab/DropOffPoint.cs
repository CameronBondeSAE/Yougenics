using System;
using System.Collections;
using System.Collections.Generic;
using Kevin;
using Ollie;
using Sirenix.OdinInspector;
using UnityEngine;

[SelectionBase]
public class DropOffPoint : MonoBehaviour, IEnergyDrainer, IItem
{
    
    public Energy energy;
    public Energy gameManagerEnergy;
    private bool currentlyDraining;
    public List<GameObject> energyContainers;
    public float drainRate;
    public GameManager gameManager;
    public ItemInfo itemInfo;

    public Renderer cube;
    public Material inactive;
    public Material active;
    

    void Awake()
    {
        
        //energy = GetComponent<Energy>();
        energyContainers = new List<GameObject>();
        StartCoroutine(DrainCoroutine());
    }

    void Start()
    {
        GameManager.instance.dropOffPoints.Add(gameObject);
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
                    
                    cube.material = active;
                }
                
                else if  (target.GetComponent<Energy>().EnergyAmount.Value <= 0)
                {
                    currentlyDraining = false;
                    cube.material = inactive;
                }
            }
            
        }
        else
        {
            currentlyDraining = false;
            cube.material = inactive;
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

    public void SpawnedAsNormal()
    {
        throw new NotImplementedException();
    }

    public ItemInfo GetInfo()
    {
        return itemInfo;
    }
}
