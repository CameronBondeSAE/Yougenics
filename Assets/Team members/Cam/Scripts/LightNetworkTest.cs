using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class LightNetworkTest : MonoBehaviour
{
	public Light light;
	
	// Start is called before the first frame update
	void Start()
	{
	}

	// Update is called once per frame
	void Update()
	{
		if (InputSystem.GetDevice<Keyboard>().spaceKey.wasPressedThisFrame)
		{
			ToggleLight(!light.enabled);
		}
	}

	public void ToggleLight(bool state)
	{
		light.enabled = state;
	}
	
	
	
	
	[ServerRpc]
	void RequestServerToggleLightServerRpc()
	{
		ToggleLight(!light.enabled);
		
		ToggleLightClientRpc(light.enabled);
	}

	[ClientRpc]
	public void ToggleLightClientRpc(bool state)
	{
		ToggleLight(state);
	}
}