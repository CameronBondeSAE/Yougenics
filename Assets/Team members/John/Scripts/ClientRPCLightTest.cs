using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class ClientRPCLightTest : NetworkBehaviour
{
    bool updateLights = true;
    Light light;

    public override void OnNetworkSpawn()
    {
        light = GetComponent<Light>();

        transform.position = new Vector3(Random.Range(-5, 5), 0, Random.Range(-5, 5));

        if (IsOwner)
        {
            ChangeLights();
        }
    }

    public void ChangeLights()
    {
        if (IsServer)
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
            Vector3 newLightColour = GetRandomColour();
            ChangeColourClientRpc(newLightColour);
            yield return new WaitForSeconds(2f);
        }
        while (updateLights);
    }

    static Vector3 GetRandomColour()
    {
        return new Vector3(Random.Range(0f, 255f), Random.Range(0f, 255f), Random.Range(0f, 255f));
    }

    [ClientRpc]
    public void ChangeColourClientRpc(Vector3 newColour)
    {
        light.color = new Color(newColour.x, newColour.y, newColour.z);
    }
}
