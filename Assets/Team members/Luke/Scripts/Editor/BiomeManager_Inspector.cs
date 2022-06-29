using System;
using Luke;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

[CustomEditor(typeof(BiomeManager))]
public class BiomeManager_Inspector : Editor
{
	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();

		if (GUILayout.Button("Fill Grid"))
		{
			((BiomeManager) target).FillGrid();
		}

		if (GUILayout.Button("FastForward"))
		{
			Time.timeScale = 2;
		}
		
		if (GUILayout.Button("Normal"))
		{
			Time.timeScale = 1;
		}
	}
}
