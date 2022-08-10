using Minh;
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

        if (GUILayout.Button("interact"))
        {
            Interactf healing = target as Interactf;
            healing.Healing();
        }
        if (GUILayout.Button("dealdmg"))
        {
            Interactf dealdamage = target as Interactf;
            dealdamage.Interact();
        }
        
    }
    
}
