using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DoorModel : MonoBehaviour, IInteractable
{

    bool isOpen = false;

    public event Action<bool> onDoorInteractEvent;

    public void Interact()
    {
        if(isOpen)
        {
            isOpen = false;
        }
        else
        {
            isOpen = true;
        }

        onDoorInteractEvent?.Invoke(isOpen);
    }
}
