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
        }
    }
}