using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Luke
{
    public interface ILukeEdible
	{
		public delegate void RemoveFromListAction(Transform transform);
		public event RemoveFromListAction RemoveFromListEvent;

        public void CallRemoveEvent(Transform transform);
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
		public float metabolism;
		public float firstMatingDelay;
		public bool isCarnivore;
		public CreatureBase.Sex gender;
		public float dangerLevel;
		public float visionRadius;
	}
}