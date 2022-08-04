using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FakeWall : MonoBehaviour
{
	// eg Smashed, moved, raised etc
	public void WallUpdated()
	{
		Utilities.OnWorldObstacleUpdatedEvent(gameObject, GetComponent<Collider>().bounds);
	}
}
