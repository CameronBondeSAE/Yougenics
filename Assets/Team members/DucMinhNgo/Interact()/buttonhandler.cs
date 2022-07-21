using System;
using System.Collections;
using System.Collections.Generic;
using Minh;
using Unity.VisualScripting;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Interactf))]
public class buttonhandler : Editor
{
    private Interactf heal;
    

    public override void OnInspectorGUI()
    {
        
        heal = (Interactf)target;
        base.OnInspectorGUI();

        if (GUILayout.Button("heal"))
        {
            Interactf healing = target as Interactf;
            healing.Interact();
        }
        if (GUILayout.Button("damaged"))
        {
            Interactf dying = target as Interactf;
            dying.Dealdamage();
        }
    }
    
}
