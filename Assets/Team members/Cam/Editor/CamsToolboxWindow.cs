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
		
		
		if (GUILayout.Button("Fold all open hierarchy"))
		{
			
		}
	}
}
