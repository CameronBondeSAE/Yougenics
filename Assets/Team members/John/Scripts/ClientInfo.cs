using Unity.Netcode;
using UnityEngine;

public class ClientInfo : NetworkBehaviour
{

    public string clientName = "Player";

    public override void OnNetworkSpawn()
    {
        if (IsOwner)
        {
            
        }
    }
}
