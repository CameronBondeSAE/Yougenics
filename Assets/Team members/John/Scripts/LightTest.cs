using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class LightTest : NetworkBehaviour
{
    bool updateLights = true;
    Light light;

    public NetworkVariable<Vector4> LightColour = new NetworkVariable<Vector4>();

    public override void OnNetworkSpawn()
    {
        light = GetComponent<Light>();

        if (IsOwner)
        {
            ChangeLights();
        }
    }

    public void ChangeLights()
    {
        if(IsServer)
        {
            StartCoroutine(ChangeLightColour());
        }
        else
        {
            SubmitLightRequestServerRpc();
        }
    }

    [ServerRpc]
    void SubmitLightRequestServerRpc()
    {
        StartCoroutine(ChangeLightColour());
    }

    IEnumerator ChangeLightColour()
    {
        do
        {
            Vector4 newLightColour = GetRandomColour();
            light.color = newLightColour;
            LightColour.Value = newLightColour;
            ChangeColourClientRpc(newLightColour);
            yield return new WaitForSeconds(2f);
        }
        while (updateLights);
    }

    static Vector4 GetRandomColour()
    {
        return new Vector4(Random.Range(0f, 255f), Random.Range(0f, 255f), Random.Range(0f, 255f), 255f);
    }

    [ClientRpc]
    public void ChangeColourClientRpc(Vector4 newColour)
    {
        light.color = LightColour.Value;
    }

}
