using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class NetworkClient_Disabler : MonoBehaviour
{
    public List<Component> disableModelComponents;

    public List<UnityEvent> doTheseAswell;

    void Awake()
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
