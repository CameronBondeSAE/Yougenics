using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamSmash : MonoBehaviour
{
	void OnTriggerStay(Collider other)
	{
		Health health = other.GetComponent<Health>();
		if (health != null)
		{
			health.Hp -= 100f;
		}
	}
}
