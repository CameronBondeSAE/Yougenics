using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Unity.Netcode;

public class NetworkClient_Disabler : NetworkBehaviour
{
    public List<Component> disableModelComponents;

    public List<UnityEvent> doTheseAswell;

    public override void OnNetworkSpawn()
    {
        if(!IsOwner)
        {
            foreach (Component component in disableModelComponents)
            {
                Destroy(component);
            }

            foreach (UnityEvent unityEvent in doTheseAswell)
            {
                unityEvent?.Invoke();
            }
        }
    }
}
