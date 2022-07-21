using NodeCanvas.Tasks.Actions;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

public class ToolBoxEditor : EditorWindow
{
    // Add menu named "My Window" to the Window menu
    [MenuItem("Tools/Alex's toolbox")]
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
    
        if (GUILayout.Button("Focus on random drone"))
        {
            DroneModel[] findObjectsOfType = FindObjectsOfType<DroneModel>();
            Selection.activeGameObject = findObjectsOfType[Random.Range(0, findObjectsOfType.Length)].gameObject;
            SceneView.FrameLastActiveSceneView();
        }
        
        if (GUILayout.Button("Focus on random AI below 50 energy(Alex'sAI)"))
        {
            
            Alex.AnotherAI[] findObjectsOfType = FindObjectsOfType<Alex.AnotherAI>();
            Selection.activeGameObject = findObjectsOfType[Random.Range(0, findObjectsOfType.Length)].gameObject;
            SceneView.FrameLastActiveSceneView();
        }

        if (GUILayout.Button("Focus on random object below 50 energy"))
        {
            Energy[] findObjectsOfType = FindObjectsOfType<Energy>();
            foreach (Energy energy in findObjectsOfType)
            {
                if (energy.energyAmount < 50)
                {
                    Selection.activeGameObject = findObjectsOfType[Random.Range(0, findObjectsOfType.Length)].gameObject;
                }
            }
        }

    }
}