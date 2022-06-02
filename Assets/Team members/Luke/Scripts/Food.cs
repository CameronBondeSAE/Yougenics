using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Luke
{
	public class Food : MonoBehaviour
	{
		public event Amenities.RemoveFromListAction RemoveFromListEvent;

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