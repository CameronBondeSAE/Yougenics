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
	public Renderer crystals1;
	public Renderer crystals2;
	public List<Color> crystalColours;

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
		crystals1.material.SetColor("_Crystal_Colour", crystalColours[(int)type]);
		crystals2.material.SetColor("_Crystal_Colour", crystalColours[(int)type]);
	}
}
