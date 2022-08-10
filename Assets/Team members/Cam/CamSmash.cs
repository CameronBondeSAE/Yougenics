using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamSmash : MonoBehaviour
{
	void OnTriggerStay(Collider other)
	{
		Minh.Health health = other.GetComponent<Minh.Health>();
		if (health != null)
		{
			//health.Hp -= 100;
		}
	}
}
