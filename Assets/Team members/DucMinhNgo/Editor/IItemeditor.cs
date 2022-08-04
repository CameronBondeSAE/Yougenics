using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Minh;

[CustomEditor(typeof(Iitemitem), true)]
public class IItemeditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        
        if (GUILayout.Button("Do Thing"))
        {
            (target as Iitemitem)?.DoThing();
        }
    }
}