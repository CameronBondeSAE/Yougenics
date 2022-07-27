using Luke;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class CritterViewModel : NetworkBehaviour
{
	[SerializeField]
	public ParticleSystemRenderer ps;
	[SerializeField]
	public List<Material> psMats;

	public Critter critter;

    public override void OnNetworkSpawn()
	{
		if (IsServer)
		{
			critter.ChangeEmotionEvent += ChangeEmotionClientRpc;
		}
	}

	[ClientRpc]
	public void ChangeEmotionClientRpc(Critter.Emotions type)
	{
		ps.material = psMats[(int)type];
	}
}
