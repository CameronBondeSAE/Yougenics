using Unity.Netcode;
using UnityEngine;

public class ClientInfo : NetworkBehaviour
{

    public string name = "test ";

    public override void OnNetworkSpawn()
    {
        if (IsOwner)
        {
            
        }
    }
}
