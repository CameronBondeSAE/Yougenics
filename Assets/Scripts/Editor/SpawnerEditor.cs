using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Spawner))]
public class SpawnerEditor : Editor
{
	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();

		if (GUILayout.Button("Spawn multiple"))
		{
			Spawner spawner = target as Spawner;

			if (spawner != null) spawner.SpawnMultiple();
		}
	}
}
