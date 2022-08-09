using System.Collections;
using System.Collections.Generic;
using Kevin;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(AStarManager))]
public class AStarEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        AStarManager aStar = (AStarManager) target;
        /*if(GUILayout.Button("Random Path"))
        {
            AStarManager aStar = (AStarManager) target;
            aStar.startPosition = aStar.RandomiseStartLocation();
            aStar.endPosition = aStar.RandomiseEndLocation();
        }*/

        if (GUILayout.Button("Pathfinding Start"))
        {
            //AStarManager aStar = (AStarManager) target;
            //aStar.PathfindingStart();
        }
        
        /*if (GUILayout.Button("Reset Nodes"))
        {
            AStarManager aStar = (AStarManager) target;
            aStar.Restart();
        }*/

        if (GUILayout.Button("Check Neighbours"))
        {
            aStar.CheckNeighbours(); 
        }
    }
}
