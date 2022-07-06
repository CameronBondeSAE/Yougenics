using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableTest : MonoBehaviour, IInteractable
{
	public void Interact()
	{
		transform.DOPunchScale(new Vector3(2f, 2f, 2f), 0.5f);
	}
}
