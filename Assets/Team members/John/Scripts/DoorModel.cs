using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.Netcode;

public class DoorModel : NetworkBehaviour, IInteractable
{
    [SerializeField]
    bool isOpen = false;
    public NetworkVariable<bool> IsDoorOpen = new NetworkVariable<bool>();

    public event Action<bool> onDoorInteractEvent;

    //SUPER HACK
    public BoxCollider closedCollider;
    public BoxCollider openCollider;

    public override void OnNetworkSpawn()
    {
        IsDoorOpen.OnValueChanged += UpdateDoorState;
    }

    private void UpdateDoorState(bool previousValue, bool newValue)
    {
        isOpen = newValue;
        onDoorInteractEvent?.Invoke(isOpen);
    }

    public void Interact()
    {
        //Non-Network Functionality
        if(NetworkManager.Singleton == null)
        {            
            Debug.Log("Non-Network Interaction");
            isOpen = !isOpen;
            onDoorInteractEvent?.Invoke(isOpen);
            return;
        }

        //Networked
        if(IsServer)
        {
            if (isOpen)
            {
                IsDoorOpen.Value = false;
            }
            else
            {
                IsDoorOpen.Value = true;
            }

            UpdateColliderHackClientRpc(IsDoorOpen.Value);
        }
        else
        {
            if(IsOwner)
                SubmitInteractRequestServerRpc();
        }
    }

    [ServerRpc]
    private void SubmitInteractRequestServerRpc()
    {
        Interact();
    }

    [ClientRpc]
    public void UpdateColliderHackClientRpc(bool doorOpen)
    {
        if(doorOpen)
        {
            closedCollider.enabled = false;
            openCollider.enabled = true;
        }
        else
        {
            closedCollider.enabled = true;
            openCollider.enabled = false;
        }
    }
}
