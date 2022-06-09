using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class NetworkVariableLightTest : NetworkBehaviour
{
    bool updateLights = true;
    Light light;

    public NetworkVariable<Vector3> LightColour = new NetworkVariable<Vector3>();
    public NetworkVariable<Vector3> MyPosition = new NetworkVariable<Vector3>();

    public override void OnNetworkSpawn()
    {
        LightColour.OnValueChanged += UpdateMyColour;
        MyPosition.OnValueChanged += UpdateMyPosition;

        light = GetComponent<Light>();

        if (IsOwner)
        {
            ChangeLights();
            SetPosition();
        }
    }

    private void UpdateMyPosition(Vector3 previousValue, Vector3 newValue)
    {
        transform.position = newValue;
    }

    private void UpdateMyColour(Vector3 previousValue, Vector3 newValue)
    {
        light.color = new Color(newValue.x, newValue.y, newValue.z);
    }

    public void ChangeLights()
    {
        if(IsServer)
        {
            //StartCoroutine(ChangeLightColour());
        }


        /*
        else
        {
            SubmitLightRequestServerRpc();
        }
        */
    }

    private void OnMouseDown()
    {
        if(IsOwner)
        {
            SubmitLightRequestServerRpc();
        }
    }

    public void SetPosition()
    {
        if (IsServer)
        {
            MyPosition.Value = GetRandomPosition();
        }
        else
        {
            SubmitPositionRequestServerRpc();
        }
    }

    [ServerRpc]
    void SubmitLightRequestServerRpc()
    {
        //StartCoroutine(ChangeLightColour());
        LightColour.Value = GetRandomColour();
    }

    [ServerRpc]
    void SubmitPositionRequestServerRpc()
    {
        MyPosition.Value = GetRandomPosition();
    }

    IEnumerator ChangeLightColour()
    {
        do
        {
            LightColour.Value = GetRandomColour();
            yield return new WaitForSeconds(2f);
        }
        while (updateLights);
    }

    static Vector3 GetRandomColour()
    {
        return new Vector3(Random.Range(0f, 255f), Random.Range(0f, 255f), Random.Range(0f, 255f));
    }
    static Vector3 GetRandomPosition()
    {
        return new Vector3(Random.Range(-5, 5), 0, Random.Range(-5, 5));
    }
}
