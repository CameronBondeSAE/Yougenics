using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.Netcode;

public class DoorModel : NetworkBehaviour, IInteractable
{

    bool isOpen = false;

    public event Action<bool> onDoorInteractEvent;

    public void Interact()
    {
        if(IsServer)
        {
            if (isOpen)
            {
                isOpen = false;
            }
            else
            {
                isOpen = true;
            }

            onDoorInteractEvent?.Invoke(isOpen);
        }
        else
        {
            SubmitInteractRequestClientRpc();
        }
    }

    [ClientRpc]
    private void SubmitInteractRequestClientRpc()
    {
        Interact();
    }
}
