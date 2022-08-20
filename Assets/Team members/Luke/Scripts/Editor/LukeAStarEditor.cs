using System;
using Luke;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(LukeAStarManager))]
public class LukeAStarEditor : Editor
{
	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();

		if (GUILayout.Button("Randomise Start Location"))
		{
			LukeAStarManager aStarManager = (LukeAStarManager) target;
            aStarManager.startLocation = aStarManager.RandomLocation();
            // aStarManager.ResetNodes();
		}

		if (GUILayout.Button("Randomise End Location"))
		{
            LukeAStarManager aStarManager = (LukeAStarManager) target;
            aStarManager.endLocation = aStarManager.RandomLocation();
            // aStarManager.ResetNodes();
		}
        
        /*if (GUILayout.Button("Begin AStar Algorithm"))
        {
	        LukeAStarManager aStarManager = (LukeAStarManager) target;
            if (!aStarManager.slowMode)
            {
                aStarManager.breaker = true;
                    ((LukeAStarManager) target).AStarAlgorithmFast();
            }
            else
            {
                if(aStarManager.coroutineInstance != null) aStarManager.StopCoroutine(aStarManager.coroutineInstance);
                aStarManager.coroutineInstance = aStarManager.StartCoroutine(((LukeAStarManager) target).AStarAlgorithm());
            }
        }*/
        
        /*if (GUILayout.Button("Reset Nodes"))
        {
	        LukeAStarManager aStarManager = (LukeAStarManager) target;
	        aStarManager.ResetNodes();
        }*/
	}
}
