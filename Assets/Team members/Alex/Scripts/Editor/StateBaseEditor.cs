using System.Collections;
using System.Collections.Generic;
using Alex;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(StateBase), true)]
public class StateBaseEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Sleeping"))
        {
            (target as Sleeping)?.OnEnable();
        }
        
        if (GUILayout.Button("Wandering"))
        {
            (target as Wandering)?.OnEnable();
        }
        if (GUILayout.Button("Looking for food"))
        {
            (target as LookingForFood)?.OnEnable();
        }
        GUILayout.EndHorizontal();
    }
}
