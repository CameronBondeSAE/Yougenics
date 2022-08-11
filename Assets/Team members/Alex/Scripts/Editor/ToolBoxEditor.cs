using System.Collections.Generic;
using Alex;
using NodeCanvas.Tasks.Actions;
using Ollie;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

public class ToolBoxEditor : EditorWindow
{
    public GameObject vehicle;
    public GameObject drone;
    public GameObject energyContainer;
    public GameObject dropOffPoint;
    public GameObject energyBall;
    
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

        if (GUILayout.Button("Make small"))
        {
            if (Selection.transforms != null)
            {
                // Log any changes to things outside of Play mode, so the user can undo
                Undo.RecordObjects(Selection.transforms, "Make small");

                foreach (Transform t in Selection.transforms)
                {
                    t.localScale = t.localScale * 0.5f;
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
        
        if (GUILayout.Button("Focus on random vehicle"))
        {
            VehicleModel[] findObjectsOfType = FindObjectsOfType<VehicleModel>();
            Selection.activeGameObject = findObjectsOfType[Random.Range(0, findObjectsOfType.Length)].gameObject;
            SceneView.FrameLastActiveSceneView();
        }
        
        if (GUILayout.Button("Focus on random Item Shop"))
        {
            ShopSingleItem[] findObjectsOfType = FindObjectsOfType<ShopSingleItem>();
            Selection.activeGameObject = findObjectsOfType[Random.Range(0, findObjectsOfType.Length)].gameObject;
            SceneView.FrameLastActiveSceneView();
        }
        
        if (GUILayout.Button("Focus on random Drop Off Point"))
        {
            DropOffPoint[] findObjectsOfType = FindObjectsOfType<DropOffPoint>();
            Selection.activeGameObject = findObjectsOfType[Random.Range(0, findObjectsOfType.Length)].gameObject;
            SceneView.FrameLastActiveSceneView();
        }
        
        if (GUILayout.Button("Focus on random Energy Container"))
        {
            EnergyContainer[] findObjectsOfType = FindObjectsOfType<EnergyContainer>();
            Selection.activeGameObject = findObjectsOfType[Random.Range(0, findObjectsOfType.Length)].gameObject;
            SceneView.FrameLastActiveSceneView();
        }
        
        if (GUILayout.Button("Focus on random Energy Ball"))
        {
            EnergyBall[] findObjectsOfType = FindObjectsOfType<EnergyBall>();
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
                if (energy.EnergyAmount.Value < 20)
                {
                    //Adding to the low energy list
                    thingsWithLowEnergy.Add(energy);
                }
            }
            //Selecting random object from the low energy list and focusing on it
            Selection.activeGameObject = thingsWithLowEnergy[Random.Range(0, thingsWithLowEnergy.Count)].gameObject;
            SceneView.FrameLastActiveSceneView();
        }
        
        drone = (GameObject)EditorGUILayout.ObjectField("Drone", drone, typeof(GameObject), false);
        if (GUILayout.Button("Create drone"))
        {
            Camera sceneCamera = SceneView.GetAllSceneCameras()[0];
            Instantiate(drone, sceneCamera.transform.position + sceneCamera.transform.forward * 5f, sceneCamera.transform.rotation);
        }
        
        vehicle = (GameObject)EditorGUILayout.ObjectField("Vehicle", vehicle, typeof(GameObject), false);
        if (GUILayout.Button("Create vehicle"))
        {
            Camera sceneCamera = SceneView.GetAllSceneCameras()[0];
            Instantiate(vehicle, sceneCamera.transform.position + sceneCamera.transform.forward * 5f, sceneCamera.transform.rotation);
        }
        
        dropOffPoint = (GameObject)EditorGUILayout.ObjectField("Drop Off Point", dropOffPoint, typeof(GameObject), false);
        if (GUILayout.Button("Create drop off point"))
        {
            Camera sceneCamera = SceneView.GetAllSceneCameras()[0];
            Instantiate(dropOffPoint, sceneCamera.transform.position + sceneCamera.transform.forward * 5f, sceneCamera.transform.rotation);
        }
        
        energyContainer = (GameObject)EditorGUILayout.ObjectField("Energy Container", energyContainer, typeof(GameObject), false);
        if (GUILayout.Button("Create energy container"))
        {
            Camera sceneCamera = SceneView.GetAllSceneCameras()[0];
            Instantiate(energyContainer, sceneCamera.transform.position + sceneCamera.transform.forward * 5f, sceneCamera.transform.rotation);
            
        }
        
        energyBall = (GameObject)EditorGUILayout.ObjectField("Energy Ball", energyBall, typeof(GameObject), false);
        if (GUILayout.Button("Create energy ball"))
        {
            Camera sceneCamera = SceneView.GetAllSceneCameras()[0];
            Instantiate(energyBall, sceneCamera.transform.position + sceneCamera.transform.forward * 5f, sceneCamera.transform.rotation);
            
        }
    }
}