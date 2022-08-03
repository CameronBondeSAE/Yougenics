using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyDrainer : MonoBehaviour
{
    private Energy energy;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void DrainEnergy(Vector3 center, float drainRange)
    {
        Collider[] hitColliders = Physics.OverlapSphere(center, drainRange);
        foreach (var hitCollider in hitColliders)
        {

        }
    }
}
