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
        if (GUILayout.Button("Give Energy"))
        {
            (target as Energy)?.ChangeEnergy(10f);
        }
        
        if (GUILayout.Button("Take Energy"))
        {
            (target as Energy)?.ChangeEnergy(-10f);
        }
        
        if (GUILayout.Button("Full Energy"))
        {
            (target as Energy)?.CheckEnergyMax();
        }
        if (GUILayout.Button("Energy Draining"))
        {
            (target as Energy)?.CheckEnergyMin();
        }
        GUILayout.EndHorizontal();
    }
}
