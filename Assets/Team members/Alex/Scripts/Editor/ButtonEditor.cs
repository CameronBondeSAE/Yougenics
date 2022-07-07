using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Alex;

[CustomEditor(typeof(Button), true)]
public class ButtonEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        
        if (GUILayout.Button("Press"))
        {
            (target as Button)?.Press();
        }
    }
}
