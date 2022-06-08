using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Luke
{
	public interface IEdible
	{
		public delegate void RemoveFromListAction(Transform transform);
		public event RemoveFromListAction RemoveFromListEvent;
	}
	
	public enum DefaultBehaviours
	{
		Lovelorn,
		Tired,
		Hungry
	}

	public enum BestNeighbourBiome
	{
		Current,
		North,
		East,
		South,
		West
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
		public float regularMatingDelay;
		public bool isCarnivore;
		public int deadliness;
		public float visionRadius;
	}
}