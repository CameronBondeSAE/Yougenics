using System;
using Luke;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(LukeAStar))]
public class LukeAStarEditor : Editor
{
	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();

		if (GUILayout.Button("Randomise Start Location"))
		{
			((LukeAStar) target).startLocation = (Vector3)((LukeAStar) target)?.RandomLocation();
		}

		if (GUILayout.Button("Randomise End Location"))
		{
            ((LukeAStar) target).endLocation = (Vector3)((LukeAStar) target)?.RandomLocation();
		}
        
        if (GUILayout.Button("Begin AStar Algorithm"))
        {
            ((LukeAStar) target).StartCoroutine(((LukeAStar) target).AStarAlgorithm());
        }
	}
}
