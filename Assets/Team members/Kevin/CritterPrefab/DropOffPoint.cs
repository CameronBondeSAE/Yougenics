using System;
using System.Collections;
using System.Collections.Generic;
using Kevin;
using Ollie;
using Sirenix.OdinInspector;
using UnityEngine;

public class DropOffPoint : MonoBehaviour
{
    public Energy energy;
    public Energy gameManagerEnergy;
    
    // HACK change from stay to a corouting or something
    public void OnTriggerStay(Collider other)
    {
        EnergyContainer otherEnergy = other.GetComponent<EnergyContainer>();
        if (otherEnergy != null)
        {
            // TODO remove energy from container gradually
            
            // TODO put "energy container" energy into our energy component
            
        }
    }

    void FixedUpdate()
    {
        // TODO Update gameManagers' energy over time here
        
    }
}
