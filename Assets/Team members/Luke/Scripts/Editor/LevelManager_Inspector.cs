using System;
using Luke;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

[CustomEditor(typeof(LevelManager))]
public class LevelManager_Inspector : Editor
{
	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();

		if (GUILayout.Button("Scan World"))
		{
			((LevelManager) target).ScanWorld();
		}

		if (GUILayout.Button("Fill World"))
		{
			((LevelManager) target).FillWorld();
		}
	}
}
