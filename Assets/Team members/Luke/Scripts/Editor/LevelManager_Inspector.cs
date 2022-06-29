using System;
using Luke;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

[CustomEditor(typeof(WorldFlooder))]
public class LevelManager_Inspector : Editor
{
	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();

		if (GUILayout.Button("Scan World"))
		{
			((WorldFlooder) target).ScanWorld();
		}

		if (GUILayout.Button("Fill World"))
		{
			((WorldFlooder) target).FillWorld();
		}
	}
}
