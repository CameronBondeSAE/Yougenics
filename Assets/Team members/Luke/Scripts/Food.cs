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
		public float health;
		[SerializeField]
		private float maxHealth = 100f;

		private void OnEnable()
		{
			health = maxHealth;
		}

		void OnDisable()
		{
			RemoveFromListEvent?.Invoke(transform);
		}
		
		void OnDestroy()
		{
			RemoveFromListEvent?.Invoke(transform);
		}
		
		public void TakeDamage(float damage)
		{
			health -= damage;
			if (health > 0) return;
			Destroy(gameObject);
		}
	}
}