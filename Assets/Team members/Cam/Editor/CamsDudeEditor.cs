using Cam;
using NodeCanvas.Tasks.Actions;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CamsDude))]
public class CamsDudeEditor : Editor
{
	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();


		GUILayout.BeginHorizontal();

		if (GUILayout.Button("Do Thing"))
		{
			// Short casting example
			// (target as CamsDude)?.DoThing();

			
			// Long casting example
			CamsDude camsDude;
			camsDude = target as CamsDude;
			
			if (camsDude != null)
			{
				camsDude.DoThing();
			}
		}

		if (GUILayout.Button("Cams super death"))
		{
			(target as CamsDude)?.CamSuperDeath();
		}
		
		GUILayout.EndHorizontal();
	}
}