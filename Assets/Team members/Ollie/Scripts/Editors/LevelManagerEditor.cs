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
            
            if (GUILayout.Button("Spawn Water"))
            {
                ((LevelManager) target).SpawnWater();
            }
            
            if (GUILayout.Button("Fill Neighbours"))
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