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
        Energy energy = (target as Energy);
        
        if (GUILayout.Button("Give Energy"))
        {
            energy?.ChangeEnergy(10f);
        }
        
        if (GUILayout.Button("Take Energy"))
        {
            energy?.ChangeEnergy(-10f);
        }
        
        if (GUILayout.Button("Full Energy"))
        {
            energy?.ChangeEnergy(100000000);
        }        
        if (GUILayout.Button("Suck Energy"))
        {
            energy?.ChangeEnergy(-100000000);
        }
        GUILayout.EndHorizontal();
    }
}
