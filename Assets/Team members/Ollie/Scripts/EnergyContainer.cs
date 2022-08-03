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
        StartCoroutine(DrainCoroutine());
    }

    private void Update()
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

    // private void OnTriggerStay(Collider other)
    // {
    //     if (!currentlyDraining && drainTargets.Count > 0)
    //     {
    //         StartCoroutine(DrainCoroutine(other));
    //     }
    //     
    // }

    IEnumerator DrainCoroutine()
    {
        currentlyDraining = true;
        if (drainTargets.Count > 0)
        {
            foreach (GameObject target in drainTargets)
            {
                energy.ChangeEnergy(target.GetComponent<Energy>().ChangeEnergy(-drainRate));
                print("yoink");
            }
        }

        yield return new WaitForSeconds(1f);
        StartCoroutine(DrainCoroutine());
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
