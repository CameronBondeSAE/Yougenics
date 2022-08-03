using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Minh;
using NodeCanvas.Tasks.Actions;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

public class ToolBoxEditor : EditorWindow
{
    // Add menu named "My Window" to the Window menu
    [MenuItem("Tools/Minh's toolbox")]
    static void Init()
    {
        // Get existing open window or if none, make a new one:
        ToolBoxEditor window = (ToolBoxEditor)EditorWindow.GetWindow(typeof(ToolBoxEditor));
        window.Show();
    }

    void OnGUI()
    {
        if (GUILayout.Button("Make big"))
        {
            if (Selection.transforms != null)
            {
                // Log any changes to things outside of Play mode, so the user can undo
                Undo.RecordObjects(Selection.transforms, "Make big");

                foreach (Transform t in Selection.transforms)
                {
                    t.localScale = t.localScale * 2;
                }
            }
        }

        if (GUILayout.Button("Undo"))
        {
            Undo.PerformUndo();
        }
    
        if (GUILayout.Button("Focus on base car"))
        {
            Minh.Controllercar[] findObjectsOfType = FindObjectsOfType<Controllercar>();
            Selection.activeGameObject = findObjectsOfType[Random.Range(0, findObjectsOfType.Length)].gameObject;
            SceneView.FrameLastActiveSceneView();
        }

        if (GUILayout.Button("Focus on object with low hp"))
        {
            FakeHp[] lowhealthcreature = FindObjectsOfType<FakeHp>();
            List<FakeHp> lowhealthscreature = new List<FakeHp>();
            
            foreach (FakeHp health in lowhealthcreature)
            {
                if (health.HP <= 10)
                {
                    Selection.activeGameObject = lowhealthcreature[Random.Range(0, lowhealthcreature.Length)].gameObject;
                    SceneView.FrameLastActiveSceneView();
                }
                else
                {
                    Selection.activeGameObject = null;
                    Debug.Log("Look at Full Hp object !");
                    
                }
            }
        }
    }
}