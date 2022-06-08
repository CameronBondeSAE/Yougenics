using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEditorInternal;
using UnityEngine;

namespace Luke
{
	public class VisionCone : MonoBehaviour
	{
		private Luke.Critter parentScript;
		private float defaultRadius;

		public void UpdateVisionRadius(float newRadius)
		{
			GetComponent<SphereCollider>().radius = newRadius;
		}
		
		void Awake()
		{
			parentScript = GetComponentInParent<Luke.Critter>();
			defaultRadius = parentScript.critterInfo.visionRadius;
			GetComponent<SphereCollider>().radius = defaultRadius;
		}
		
		void OnTriggerEnter(Collider other)
		{
			parentScript.VisionTriggerEnter(other);
		}

		// Update is called once per frame
		void OnTriggerExit(Collider other)
		{
			parentScript.VisionTriggerExit(other);
		}

		
	}
}