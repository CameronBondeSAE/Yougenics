using System;
using Luke;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(AStarUser))]
public class LukeAStarUserEditor : Editor
{
	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();

		if (GUILayout.Button("Set AStarUser"))
		{
			AStarUser aStarUser = (AStarUser) target;
			LukeAStar aStar = aStarUser.aStar;
			Vector3 startPos = aStar.startLocation;
			Vector3 endPos = aStar.endLocation;
			aStarUser.ResetNodes(startPos, endPos);
			int[] indexStart = aStar.ConvertIndexAndPosition(startPos);
			int[] indexEnd = aStar.ConvertIndexAndPosition(endPos);
			aStarUser.startNode = aStar.Nodes[indexStart[0], indexStart[1]];
			aStarUser.currentNode = aStarUser.startNode;
			aStarUser.endNode = aStar.Nodes[indexEnd[0], indexEnd[1]];
		}
        
        if (GUILayout.Button("Begin AStar Algorithm"))
        {
	        AStarUser aStarUser = (AStarUser) target;
	        aStarUser.AStarAlgorithmFast();
        }
	}
}
