using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

public class CamsToolboxWindow : EditorWindow
{
	public GameObject prefabToSpawn;
	
	
	// Add menu named "My Window" to the Window menu
	[MenuItem("Tools/Cams toolbox")]
	static void Init()
	{
		// Get existing open window or if none, make a new one:
		CamsToolboxWindow window = (CamsToolboxWindow)EditorWindow.GetWindow(typeof(CamsToolboxWindow));
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
		
		if (GUILayout.Button("Focus on random object"))
		{
			GameObject[] findObjectsOfType = FindObjectsOfType<GameObject>();
			Selection.activeGameObject = findObjectsOfType[Random.Range(0, findObjectsOfType.Length)];
			SceneView.FrameLastActiveSceneView();
		}

		GUILayout.Space(10);
		
		// Draw a draggable item widget, similar to normal drag drop stuff in the inspector
		prefabToSpawn = (GameObject)EditorGUILayout.ObjectField("Prefab To Spawn", prefabToSpawn, typeof(GameObject), true);

		if (GUILayout.Button("Spawn thing at scene camera"))
		{
			// Find the first scene camera in scene view window (not game cameras)
			Camera sceneCamera = SceneView.GetAllSceneCameras()[0];

			Instantiate(prefabToSpawn, sceneCamera.transform.position, sceneCamera.transform.rotation);
		}
		
		
		if (GUILayout.Button("Spawn thing in front of scene camera"))
		{
			// Find the first scene camera in scene view window (not game cameras)
			Camera sceneCamera = SceneView.GetAllSceneCameras()[0];

			Instantiate(prefabToSpawn, sceneCamera.transform.position + sceneCamera.transform.forward * 5f, sceneCamera.transform.rotation);
		}
		if (GUILayout.Button("Spawn thing against solid in front of scene camera"))
		{
			// Find the first scene camera in scene view window (not game cameras)
			Camera sceneCamera = SceneView.GetAllSceneCameras()[0];

			RaycastHit Hitinfo;
			if (Physics.Raycast(new Ray(sceneCamera.transform.position, sceneCamera.transform.forward), out Hitinfo, 100f, 255, QueryTriggerInteraction.Ignore))
			{
				Instantiate(prefabToSpawn, Hitinfo.point, Quaternion.identity);
			}
			
		}
		
		
		
		if (GUILayout.Button("Focus on random object below 50 energy"))
		{
			Energy[]     thingsWithEnergy    = FindObjectsOfType<Energy>();
			List<Energy> thingsWithLowEnergy = new List<Energy>();
			
			foreach (Energy energy in thingsWithEnergy)
			{
				if (energy.energyAmount < 20)
				{
					thingsWithLowEnergy.Add(energy);
				}
			}
			
			Selection.activeGameObject = thingsWithLowEnergy[Random.Range(0, thingsWithLowEnergy.Count)].gameObject;
			SceneView.FrameLastActiveSceneView();
		}
	}
}
