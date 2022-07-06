using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Cam
{


	[CustomEditor(typeof(StateBase), true)]
	public class StateBaseEditor : Editor
	{
		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();

			if (GUILayout.Button("Enter state"))
			{
				// Shortest fancy version
				//(target as StateBase)?.stateManager.ChangeState(target as StateBase);

				// Longer verbose version
				StateBase stateBase = target as StateBase;
				if (stateBase != null)
				{
					stateBase.stateManager.ChangeState(stateBase);
				}
			}
		}
	}

}