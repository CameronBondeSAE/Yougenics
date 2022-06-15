using System.Collections;
using System.Collections.Generic;
using Anthill.AI;
using UnityEngine;

namespace Luke
{
	public class LukeCritterSensor : MonoBehaviour, ISense
	{
		public event IEdible.RemoveFromListAction RemoveFromListEvent;

		public CritterInfo critterInfo;
		
		public DefaultBehaviours defaultBehaviour;
		public float health;
		public float energy;
		public float sleepLevel;
		public float acceleration;
		public float maxSpeed;
		public float regularMatingDelay;
		public bool isChild = true;
		public bool readyToMate = false;
		public bool isSleeping = false;
		public bool justAte = false;
		public bool isAttacking = false;
		
		public List<Transform> matesList;
		public List<Transform> predatorsList;
		public List<Transform> foodList;
		public List<float> biomeQualities;
		public Transform nearestPredator;
		public Transform nearestMate;
		public Transform nearestFood;
		[SerializeField]
		public BestNeighbourBiome bestNearbyBiome;
		[SerializeField]
		private Vector3 randomAdjustment;

		[SerializeField]
		private GameObject childPrefab;
		[SerializeField]
		private Transform birthingTransform;
		private Rigidbody rb;
		[SerializeField]
		private ParticleSystemRenderer ps;

		[SerializeField]
		private List<Material> psMats;
		
		public void CollectConditions(AntAIAgent aAgent, AntAICondition aWorldState)
		{
			
		}
		
		public void VisionTriggerEnter(Collider other)
		{
			if (other.transform == transform) return;
			Critter go = other.GetComponent<Critter>();
			Food go2 = other.GetComponent<Food>();
			
			if (go != null)
			{
				int otherDeadliness = go.critterInfo.deadliness;
				if (otherDeadliness >= critterInfo.deadliness + 3)
				{
					if (!predatorsList.Contains(other.transform))
					{
						predatorsList.Add(other.transform);
						go.RemoveFromListEvent += RemoveTransformFromList;
					}
				}
				else if (otherDeadliness < critterInfo.deadliness+3 && otherDeadliness > critterInfo.deadliness-3)
				{
					if (!matesList.Contains(other.transform))
					{
						Critter otherCritter = other.GetComponent<Critter>();
						if (critterInfo.gender != otherCritter.critterInfo.gender && !otherCritter.isChild)
						{
							matesList.Add(other.transform);
							go.RemoveFromListEvent += RemoveTransformFromList;
						}
					}
				}
				else if (critterInfo.isCarnivore)
				{
					if (!foodList.Contains(other.transform))
					{
						foodList.Add(other.transform);
						go.RemoveFromListEvent += RemoveTransformFromList;
					}
				}
			}
			else if (go2 != null)
			{
				if (!foodList.Contains(other.transform))
				{
					foodList.Add(other.transform);
					go2.RemoveFromListEvent += RemoveTransformFromList;
				}
			}
		}
		
		public void VisionTriggerExit(Collider other)
		{
			RemoveTransformFromList(other.transform);
		}

		private void RemoveTransformFromList(Transform _transform)
		{
			if (predatorsList.Contains(_transform))
			{
				predatorsList.Remove(_transform);
			}
			if (matesList.Contains(_transform))
			{
				matesList.Remove(_transform);
			}
			if (foodList.Contains(_transform))
			{
				foodList.Remove(_transform);
			}
		}
	}

	
}