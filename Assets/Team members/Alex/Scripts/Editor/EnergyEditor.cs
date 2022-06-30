using System.Collections;
using System.Collections.Generic;
using Alex;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Energy), true)]
public class EnergyEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("No Energy"))
        {
            (target as Energy)?.CheckEnergyMin();
        }
        
        if (GUILayout.Button("Full Energy"))
        {
            (target as Energy)?.CheckEnergyMax();
        }
        if (GUILayout.Button("Energy Draining"))
        {
            (target as Energy)?.EnergyDrainer();
        }
        GUILayout.EndHorizontal();
    }
}
