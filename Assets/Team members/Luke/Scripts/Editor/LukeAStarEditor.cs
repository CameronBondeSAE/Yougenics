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
			LukeAStar aStar = (LukeAStar) target;
			aStar.ResetNodes();
			aStar.startLocation = aStar.RandomLocation();
			int[] index = aStar.ConvertIndexAndPosition(aStar.startLocation);
			aStar.StartNode = aStar.Nodes[index[0], index[1]];
			aStar.CurrentNode = aStar.StartNode;
		}

		if (GUILayout.Button("Randomise End Location"))
		{
            LukeAStar aStar = (LukeAStar) target;
            aStar.ResetNodes();
            aStar.endLocation = aStar.RandomLocation();
			int[] index = aStar.ConvertIndexAndPosition(aStar.endLocation);
			aStar.EndNode = aStar.Nodes[index[0], index[1]];
		}
        
        if (GUILayout.Button("Begin AStar Algorithm"))
        {
	        LukeAStar aStar = (LukeAStar) target;
	        if (!aStar.slowMode) ((LukeAStar) target).AStarAlgorithmFast();
	        else aStar.coroutineInstance = aStar.StartCoroutine(((LukeAStar) target).AStarAlgorithm());
        }
        
        if (GUILayout.Button("Reset Nodes"))
        {
	        LukeAStar aStar = (LukeAStar) target;
	        aStar.ResetNodes();
        }
	}
}
