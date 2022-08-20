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
			LukeAStarManager aStarManager = aStarUser.aStarManager;
			Vector3 startPos = aStarManager.startLocation;
			Vector3 endPos = aStarManager.endLocation;
			aStarUser.ResetNodes(startPos, endPos);
			int[] indexStart = aStarManager.ConvertIndexAndPosition(startPos);
			int[] indexEnd = aStarManager.ConvertIndexAndPosition(endPos);
			aStarUser.startNode = new Vector2Int(indexStart[0], indexStart[1]);
			aStarUser.currentNode = aStarUser.startNode;
			aStarUser.endNode = new Vector2Int(indexEnd[0], indexEnd[1]);
		}
        
        if (GUILayout.Button("Begin AStar Algorithm"))
        {
	        AStarUser aStarUser = (AStarUser) target;
	        aStarUser.BeginAStarAlgorithm();
        }
	}
}
