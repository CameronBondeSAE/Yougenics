using Unity.Netcode;
using UnityEngine;
using System;
using Unity.Collections;
using UnityEngine.UI;

public class ClientInfo : NetworkBehaviour
{

    public string clientName = "Player";
    public GameObject lobbyUIRef;

    public void Init(ulong clientId)
    {
        clientName = "Player " + clientId;
    }

    /*public override void OnDestroy()
    {
        base.OnDestroy();
        Destroy(lobbyUIRef);
    }*/
}
