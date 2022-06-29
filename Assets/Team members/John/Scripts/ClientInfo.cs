using Unity.Netcode;
using UnityEngine;
using System;
using Unity.Collections;
using UnityEngine.UI;

public class ClientInfo : NetworkBehaviour
{

    public string clientName = "Player";
    //public NetworkVariable<FixedString64Bytes> ClientName = new NetworkVariable<FixedString64Bytes>();

    InputField inputField;

    public override void OnNetworkSpawn()
    {
        //inputField = FindObjectOfType<Canvas>().GetComponentInChildren<InputField>();

        if (IsOwner)
        {
            //ClientName.Value = "Player " + OwnerClientId;
        }
    }

    public void UpdateName()
    {
        if (IsServer)
        {
            //ClientName.Value = inputField.text;
        }
    }

}
