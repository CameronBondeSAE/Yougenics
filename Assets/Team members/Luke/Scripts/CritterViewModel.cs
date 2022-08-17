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

    public Coroutine coroutine;

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
        if (coroutine != null) StopCoroutine(coroutine);
		ps.material = psMats[(int)type];

        coroutine = StartCoroutine(ChangeColour(crystalColours[(int)type], 3));
    }

    public IEnumerator ChangeColour(Color colour, int iterations)
    {
        for (int i = 0; i < iterations; i++)
        {
            Color newColour = Color.Lerp(crystals1.material.GetColor("_Crystal_Colour"), colour, 0.1f);
            yield return new WaitForEndOfFrame();
            crystals1.material.SetColor("_Crystal_Colour", newColour);
            crystals2.material.SetColor("_Crystal_Colour", newColour);
        }
        
    }
}
