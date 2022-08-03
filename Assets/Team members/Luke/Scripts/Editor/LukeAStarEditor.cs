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
            aStar.startLocation = aStar.RandomLocation();
            aStar.ResetNodes();
		}

		if (GUILayout.Button("Randomise End Location"))
		{
            LukeAStar aStar = (LukeAStar) target;
            aStar.endLocation = aStar.RandomLocation();
            aStar.ResetNodes();
		}
        
        if (GUILayout.Button("Begin AStar Algorithm"))
        {
	        LukeAStar aStar = (LukeAStar) target;
            if (!aStar.slowMode)
            {
                aStar.breaker = true;
                    ((LukeAStar) target).AStarAlgorithmFast();
            }
            else
            {
                if(aStar.coroutineInstance != null) aStar.StopCoroutine(aStar.coroutineInstance);
                aStar.coroutineInstance = aStar.StartCoroutine(((LukeAStar) target).AStarAlgorithm());
            }
        }
        
        if (GUILayout.Button("Reset Nodes"))
        {
	        LukeAStar aStar = (LukeAStar) target;
	        aStar.ResetNodes();
        }
	}
}
