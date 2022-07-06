using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Ollie
{
    [CustomEditor(typeof(LevelManager))]
    [CanEditMultipleObjects]
    public class LevelManagerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (GUILayout.Button("Scan World"))
            {
                ((LevelManager) target).ScanWorld();
            }

            if (GUILayout.Button("Assign Neighbours"))
            {
                ((LevelManager) target).AssignNeighbours();
            }

            if (GUILayout.Button("Pick Random Targets"))
            {
                ((LevelManager) target).AStarPathfindingStart();
            }

            if (GUILayout.Button("Find Path"))
            {
                ((LevelManager) target).FindPath();
            }
            
            if (GUILayout.Button("Spawn Water"))
            {
                ((LevelManager) target).SpawnWater();
            }

            if (GUILayout.Button("Check Neighbours (Step 1)"))
            {
                ((LevelManager) target).CheckNeighbours();
            }
            
            if (GUILayout.Button("Add Nodes (Step 2)"))
            {
                ((LevelManager) target).AddNodes();
            }
            
            if (GUILayout.Button("Fill Neighbours (Step 3)"))
            {
                ((LevelManager) target).FillNeighbours();
            }

            if (GUILayout.Button("Print Neighbours"))
            {
                ((LevelManager) target).PrintNeighbourGridPos();
            }
        }
    }
}