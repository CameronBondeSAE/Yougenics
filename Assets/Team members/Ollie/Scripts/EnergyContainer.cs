using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyContainer : MonoBehaviour, IItem
{
    private Energy energy;
    
    public float drainRate;
    private bool currentlyDraining;
    
    public List<GameObject> drainTargets;

    private void Awake()
    {
        energy = GetComponent<Energy>();
        drainTargets = new List<GameObject>();
        currentlyDraining = false;
    }

    private void FixedUpdate()
    {
        
    }

    public void SpawnedAsNormal()
    {
        
    }

    public ItemInfo GetInfo()
    {
        throw new NotImplementedException();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Energy>() != null && !drainTargets.Contains(other.gameObject))
        {
            drainTargets.Add(other.gameObject);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (!currentlyDraining)
        {
            StartCoroutine(DrainCoroutine(other));
        }
        
    }

    IEnumerator DrainCoroutine(Collider other)
    {
        currentlyDraining = true;
        if (other.GetComponent<Energy>() != null)
        {
            if (drainTargets.Count > 0)
            {
                foreach (GameObject target in drainTargets)
                {
                    target.GetComponent<Energy>().ChangeEnergy(-drainRate);
                    energy.ChangeEnergy(drainRate);
                    print("yoink");
                }
            }
            
            if (other.GetComponent<Energy>().energyAmount <= 0)
            {
                drainTargets.Remove(other.gameObject);
            }
        }
        yield return new WaitForSeconds(1f);
        currentlyDraining = false;
    }

    private void OnTriggerExit(Collider other)
    {
        if (drainTargets.Contains(other.gameObject))
        {
            drainTargets.Remove(other.gameObject);
        }
    }
}
