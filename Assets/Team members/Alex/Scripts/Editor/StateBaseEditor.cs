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
        
        
        if (GUILayout.Button("Change to this state"))
        {
            (target as StateBase)?.GetComponent<StateManager>().ChangeState(target as StateBase);
        }
        
    }
}
