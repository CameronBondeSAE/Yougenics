using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Luke
{
	public interface IEdible
	{
		public delegate void RemoveFromListAction(Transform transform);
		public event RemoveFromListAction RemoveFromListEvent;

		public void TakeDamage(float damage);
	}

	public enum Gender
	{
		Female,
		Male,
		NonBinary
	}
	
	public enum DefaultBehaviours
	{
		Lovelorn,
		Tired,
		Hungry,
		Restless
	}

	public enum BestNeighbourBiome
	{
		Current,
		North,
		East,
		South,
		West,
		LENGTH //for utility
	}

	[System.Serializable]
	public struct CritterInfo
	{
		public float maxHealth;
		public float maxSleepLevel;
		public float maxEnergyLevel;
		public float awakeDecayDelay;
		public float asleepDecayDelay;
		public float firstMatingDelay;
		public bool isCarnivore;
		public Gender gender;
		public int deadliness;
		public float visionRadius;
	}
}