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
    public NetworkVariable<bool> IsLightActive = new NetworkVariable<bool>();

    DayNightManager dayNightManager;

    private void DayNightManagerOnPhaseChangeEvent(DayNightManager.DayPhase phase)
    {
        if(IsServer)
        {
            ChangeState(phase);
        }
        else
        {
            SubmitLightStateRequestServerRpc(phase);
        }
    }

    private void ChangeState(DayNightManager.DayPhase phase)
    {
        if (phase == DayNightManager.DayPhase.Dawn || phase == DayNightManager.DayPhase.Evening || phase == DayNightManager.DayPhase.Night ||
            phase == DayNightManager.DayPhase.Midnight)
        {
            IsLightActive.Value = true;
        }

        if (phase == DayNightManager.DayPhase.Morning ||
            phase == DayNightManager.DayPhase.Noon)
        {
            IsLightActive.Value = false;
        }
    }

    public override void OnNetworkSpawn()
    {
        light = GetComponent<Light>();

        LightColour.OnValueChanged += UpdateMyColour;
        MyPosition.OnValueChanged += UpdateMyPosition;
        IsLightActive.OnValueChanged += UpdateLightState;

        //This check is causing null errors on client end when calling line 62
        if(IsServer)
        {
            dayNightManager = FindObjectOfType<DayNightManager>();
            dayNightManager.PhaseChangeEvent += DayNightManagerOnPhaseChangeEvent;
            ChangeState(dayNightManager.CurrentPhase.Value);
        }

        if (IsOwner)
        {
            ChangeLights();
            SetPosition();
            
        }
    }

    private void UpdateLightState(bool previousValue, bool newValue)
    {
        if(light != null)
            light.enabled = newValue;
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

    [ServerRpc]
    void SubmitLightStateRequestServerRpc(DayNightManager.DayPhase phase)
    {
        ChangeState(phase);
    }

    [ServerRpc]
    void SubmitLightStateOffRequestServerRpc()
    {
        IsLightActive.Value = false;
    }

    [ServerRpc]
    void SubmitLightStateOnRequestServerRpc()
    {
        IsLightActive.Value = true;
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
