using System.Collections.Generic;
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
        
        if (GUILayout.Button("Focus on random AI"))
        {
            
            Alex.AlexAI[] findObjectsOfType = FindObjectsOfType<Alex.AlexAI>();
            Selection.activeGameObject = findObjectsOfType[Random.Range(0, findObjectsOfType.Length)].gameObject;
            SceneView.FrameLastActiveSceneView();
        }

        if (GUILayout.Button("Focus on random object below 20 energy"))
        {
            Energy[] thingsWithEnergy = FindObjectsOfType<Energy>();
            List<Energy> thingsWithLowEnergy = new List<Energy>();
            
            //Check every object that has energy
            foreach (Energy energy in thingsWithEnergy)
            {
                //Checking if energy is less than 20
                if (energy.energyAmount < 20)
                {
                    //Adding to the low energy list
                    thingsWithLowEnergy.Add(energy);
                }
            }
            //Selecting random object from the low energy list and focusing on it
            Selection.activeGameObject = thingsWithLowEnergy[Random.Range(0, thingsWithLowEnergy.Count)].gameObject;
            SceneView.FrameLastActiveSceneView();
        }
        
        if (GUILayout.Button("Create drone"))
        {
            DroneModel drone = FindObjectOfType<DroneModel>();
            Instantiate(drone);
            
            //Selection.activeGameObject = findObjectsOfType[Random.Range(0, findObjectsOfType.Length)].gameObject;
            //SceneView.FrameLastActiveSceneView();
        }
    }
}