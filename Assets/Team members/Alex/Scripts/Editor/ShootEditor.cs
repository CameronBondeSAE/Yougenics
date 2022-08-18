using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Alex;

[CustomEditor(typeof(LazerGun), true)]
public class ShootEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        
        if (GUILayout.Button("Shoot"))
        {
            (target as LazerGun)?.Interact();
        }
    }
}