using Unity.Netcode;
using UnityEngine;
using System;
using Unity.Collections;
using UnityEngine.UI;

public class ClientInfo : NetworkBehaviour
{

    public string clientName = "Player";

    public override void OnNetworkSpawn()
    {

        if (IsOwner)
        {
            //ClientName.Value = "Player " + OwnerClientId;
        }
    }

}
