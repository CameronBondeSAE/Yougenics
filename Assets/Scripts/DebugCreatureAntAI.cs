using Anthill.AI;
using System;
using TMPro;
using UnityEngine;
using Unity.Netcode;

namespace Cam.Interfaces_and_Inheritance_base_classes
{
	public class DebugCreatureAntAI : MonoBehaviour
	{
		public AntAIPlan antAIPlan;
		TextMeshPro      textMeshPro;

		public bool active = false;

		void Awake()
		{
			antAIPlan = GetComponentInParent<AntAIAgent>().currentPlan;
			GameObject o = new GameObject("Debug");
			o.transform.parent                = transform;
			textMeshPro                       = o.AddComponent<TextMeshPro>();
			textMeshPro.alignment             = TextAlignmentOptions.Bottom;
			textMeshPro.fontSize              = 6f;
			// textMeshPro.autoSizeTextContainer = true;

			RectTransform rectTransform = o.GetComponent<RectTransform>();
			rectTransform.pivot     = new Vector2(0.5f, 0f);
			// rectTransform.sizeDelta = new Vector2(8f,   100f); // = 10f;// = new Rect(0,0, 8f, 12f);

			// HACK: Assuming root has main collider
			rectTransform.localPosition = new Vector3(0, transform.GetComponent<Collider>().bounds.extents.y, 0);

			rectTransform.ForceUpdateRectTransforms();
		}

		void OnEnable()
		{
			textMeshPro.enabled = true;
		}

		void OnDisable()
		{
			textMeshPro.enabled = false;
		}

		Camera playerCam;
		void Update()
		{
			if (active)
			{
				textMeshPro.enabled = true;
				textMeshPro.text    = "";

				if (antAIPlan != null)
					foreach (string action in antAIPlan._actions)
					{
						textMeshPro.text += action + "\n";
					}

				if (NetworkManager.Singleton != null) playerCam = NetworkManager.Singleton.LocalClient.PlayerObject.GetComponent<John.PlayerController>().playerModel.GetComponentInChildren<Camera>();
				if (playerCam != null)
					textMeshPro.transform.LookAt(playerCam.transform);
			}
			else
			{
				textMeshPro.enabled = false;
			}
		}
	}
}