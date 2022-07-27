using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Alex;

[CustomEditor(typeof(ShopButton), true)]
public class ShopButtonEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        
        if (GUILayout.Button("Press"))
        {
            (target as ShopButton)?.Press();
        }
    }
}