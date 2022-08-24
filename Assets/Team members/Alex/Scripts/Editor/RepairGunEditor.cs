using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(RepairGun), true)]
public class RepairGunEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        
        if (GUILayout.Button("Shoot"))
        {
            (target as RepairGun)?.Interact();
        }
    }
}
