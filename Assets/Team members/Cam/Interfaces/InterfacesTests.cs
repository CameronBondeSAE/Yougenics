using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


// class CamsThing : MonoBehaviour, IItem, IInteractable
// {
//     
// }

public class InterfacesTests : MonoBehaviour
{
    public IItem slot1;
    
    // Start is called before the first frame update
    void Start()
    {
        // slot1 is pointing to CamsThing VIA the IItem interface, so IT ONLY KNOWS about IItem stuff

        IInteractable interactable = slot1 as IInteractable;
        
    }

    void OnTriggerStay(Collider other)
    {
        transform.LookAt(other.transform);
    }
}
