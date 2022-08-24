using Luke;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using DG.Tweening;

public class CritterViewModel : NetworkBehaviour
{
	[SerializeField]
	public ParticleSystemRenderer ps;
	[SerializeField]
	public List<Material> psMats;
	public Renderer crystals1;
	public Renderer crystals2;
	public List<Color> crystalColours;
	public GameObject shield;

    public Coroutine coroutine1;
    public Coroutine coroutine2;

	public Critter critter;

    public override void OnNetworkSpawn()
	{
		if (IsServer)
		{
			critter.ChangeEmotionEvent += ChangeEmotionClientRpc;
			critter.TakeDamageEvent += TakeDamageClientRPC;
		}

        //critter.comingOfAgeEvent += Critter_comingOfAgeEvent;
	}

    /*private void Critter_comingOfAgeEvent()
    {
        critter.transform.Translate(Vector3.up * 0.5f);
        transform.DOPunchScale(transform.localScale * 1.5f, 0.5f);
    }

    private void FixedUpdate()
    {
		SetScale();
    }

	private void SetScale()
	{
		//include eating in equation
		float scale = Mathf.Min(0.5f + (critter.maxSize - 0.5f) * critter.age / critter.maxAge, critter.maxSize);
		transform.localScale = Vector3.one * scale;
	}*/

	[ClientRpc]
	public void ChangeEmotionClientRpc(Critter.Emotions type)
	{
        if (coroutine1 != null) StopCoroutine(coroutine1);
		ps.material = psMats[(int)type];

        coroutine1 = StartCoroutine(ChangeColour(crystalColours[(int)type], 3));
    }

	[ClientRpc]
	public void TakeDamageClientRPC()
	{
		if (coroutine2 != null) StopCoroutine(coroutine2);
		coroutine2 = StartCoroutine(ToggleShieldEffect());
	}

	private IEnumerator ToggleShieldEffect()
	{
		shield.SetActive(true);
		yield return new WaitForSeconds(2f);
		shield.SetActive(false);
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
