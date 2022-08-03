using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utilities
{
	public static Vector3 FindGroundHeight(Vector3 startPosition, float _groundOffset)
	{
		// Start much higher
		Vector3 startRay = startPosition + Vector3.up * 500f;

		Ray ray = new Ray(startRay, Vector3.down);

		if (Physics.Raycast(ray, out RaycastHit hit, 1000f, 255, QueryTriggerInteraction.Ignore))
		{
			// Add offset from detected ground point
			Vector3 newSpawnPos = hit.point + Vector3.up * _groundOffset;
			// Debug.DrawRay(startRay, Vector3.down * hit.distance, Color.blue);
			// Debug.Log(hit.distance);

			// newSpawnPos = new Vector3(newSpawnPos.x,
			// newSpawnPos.y - (hit.distance - groundOffset),
			// newSpawnPos.z);

			return newSpawnPos;
		}
		else
		{
			return Vector3.zero;
		}
	}
}