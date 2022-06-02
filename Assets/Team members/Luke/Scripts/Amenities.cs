using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Luke
{
	public enum DefaultBehaviours
	{
		Lovelorn,
		Tired,
		Hungry
	}
	
	public class Amenities : MonoBehaviour
	{
		public delegate void RemoveFromListAction(Transform transform);
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