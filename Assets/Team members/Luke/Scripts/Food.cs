using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

namespace Luke
{
	public class Food : MonoBehaviour, Luke.IEdible
	{
		public event IEdible.RemoveFromListAction RemoveFromListEvent;

		void OnDisable()
		{
			RemoveFromListEvent?.Invoke(transform);
		}
		
		void OnDestroy()
		{
			RemoveFromListEvent?.Invoke(transform);
		}
		
		void Start()
		{

		}

		void Update()
		{

		}
	}
}