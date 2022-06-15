using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

namespace Luke
{
	public class Food : MonoBehaviour, Luke.IEdible
	{
		public event IEdible.RemoveFromListAction RemoveFromListEvent;

		[SerializeField]
		private float health;
		[SerializeField]
		public float maxHealth = 100f;

		private void OnEnable()
		{
			health = maxHealth;
		}

		void OnDisable()
		{
			CallRemoveEvent(transform);
		}
		
		void OnDestroy()
		{
			CallRemoveEvent(transform);
		}
		
		public void TakeDamage(float damage)
		{
			health -= damage;
			if (health > 0) return;
			Destroy(gameObject);
		}

		public void CallRemoveEvent(Transform _transform)
		{
			RemoveFromListEvent?.Invoke(_transform);
		}
	}
}