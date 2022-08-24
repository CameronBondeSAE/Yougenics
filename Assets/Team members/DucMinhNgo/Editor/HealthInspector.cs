using System.Collections;
using System.Collections.Generic;
using Minh;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Health))]
public class HealthInspector : Editor
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
        if (GUILayout.Button("starthealthincreasing"))
        {
            Health health = target as Health;
            health.startHealthincreasing();
        }
        if (GUILayout.Button("increase"))
        {
            Health health = target as Health;
            health.ChangeHealth(10f);
        }
        if (GUILayout.Button("decrease"))
        {
            Health health = target as Health;
            health.ChangeHealth(-10f);
        }
        if (GUILayout.Button("kill"))
        {
            Health health = target as Health;
            health.ChangeHealth(-1000000000f);
        }
    }
    // Start is called before the first frame update
   
}
