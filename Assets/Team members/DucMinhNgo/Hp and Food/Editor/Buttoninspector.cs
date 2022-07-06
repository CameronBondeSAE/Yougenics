using System.Collections;
using System.Collections.Generic;
using Minh;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Health))]
public class Buttoninspector : Editor
{
    private Health fullHp;
    private Health dead;

    public override void OnInspectorGUI()
    {
        
        fullHp = (Health)target;
        base.OnInspectorGUI();

        if (GUILayout.Button("Full Hp"))
        {
            Health health = target as Health;
            health.FullHp();
        }
        if (GUILayout.Button("Dead"))
        {
            Health health = target as Health;
            health.Deadtrigger();
        }
    }
    // Start is called before the first frame update
   
}
