using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beam : MonoBehaviour
{
	private void OnEnable()
	{
		Destroy(gameObject, 2f);
	}
}
