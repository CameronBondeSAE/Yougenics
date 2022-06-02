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

		void Awake()
		{
			parentScript = GetComponentInParent<Luke.Critter>();
			parentScript.critterInfo.visionRadius = GetComponent<SphereCollider>().radius;
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