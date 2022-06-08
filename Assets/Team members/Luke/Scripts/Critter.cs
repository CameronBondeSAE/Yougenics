using System;
using System.Collections;
using System.Collections.Generic;
using NodeCanvas.Framework;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

namespace Luke
{
	public class Critter : MonoBehaviour, Luke.IEdible
	{
		public event IEdible.RemoveFromListAction RemoveFromListEvent;

		public CritterInfo critterInfo;
		
		public DefaultBehaviours defaultBehaviour;
		public float health;
		public float energy;
		public float sleepLevel;
		public float acceleration;
		public float maxSpeed;
		public bool readyToMate = false;
		public bool isSleeping = false;
		public bool justAte;
		
		public List<Transform> matesList;
		public List<Transform> predatorsList;
		public List<Transform> foodList;
		public Transform nearestPredator;
		public Transform nearestMate;
		public Transform nearestFood;
		public List<int> biomeQualities;
		public int bestNearbyBiome;

		private Rigidbody rb;
		
#region IEnumerators
		
		private IEnumerator HealthRegen()
		{
			yield return new WaitForSeconds(critterInfo.awakeDecayDelay);

			if (isSleeping)
			{
				health += 1;
				if (health > critterInfo.maxHealth)
				{
					health = critterInfo.maxHealth;
				}
			}
			
			StartCoroutine(HealthRegen());
		}

		private IEnumerator EnergyDecay()
		{
			if (isSleeping)
			{
				yield return new WaitForSeconds(critterInfo.asleepDecayDelay);
			}
			else
			{
				yield return new WaitForSeconds(critterInfo.awakeDecayDelay);
			}

			if (!justAte)
			{
				if (energy > 0)
				{
					energy -= 1;
				}
				else
				{
					health -= 1;
				}
				StartCoroutine(EnergyDecay());
			}
		}

		private IEnumerator EnergyDecayCooldown()
		{
			yield return new WaitForSeconds(10f);
			justAte = false;
			
			StartCoroutine(EnergyDecay());
		}
		
		private IEnumerator SleepLevelDecay()
		{
			if (isSleeping)
			{
				yield return new WaitForSeconds(critterInfo.awakeDecayDelay);
				sleepLevel += 1;
			}
			else
			{
				yield return new WaitForSeconds(critterInfo.asleepDecayDelay);
				sleepLevel -= 1;
			}

			StartCoroutine(SleepLevelDecay());
		}
		
		private IEnumerator ComingOfAge(float delay)
		{
			yield return new WaitForSeconds(delay);

			readyToMate = true;
		}

		private IEnumerator ReadyToMateReset(float delay)
		{
			readyToMate = false;
			yield return new WaitForSeconds(delay);
			readyToMate = true;
		}

		// Every 30 seconds this runs a weighted randomisation for what the critter should do when not hungry or tired or near a mate when ready to mate. 
		private IEnumerator RandomiseDefaultBehaviour()
		{
			int lovelornWeight = readyToMate ? 3 : 0;
			int tiredWeight = Mathf.RoundToInt(5 * (sleepLevel / critterInfo.maxSleepLevel));
			int hungryWeight = Mathf.RoundToInt(5 * (energy / critterInfo.maxEnergyLevel));

			int result = UnityEngine.Random.Range(0, lovelornWeight + tiredWeight + hungryWeight);

			if (result < lovelornWeight)
			{
				defaultBehaviour = DefaultBehaviours.Lovelorn;
			}
			else if (result < lovelornWeight + tiredWeight)
			{
				defaultBehaviour = DefaultBehaviours.Tired;
			}
			else
			{
				defaultBehaviour = DefaultBehaviours.Hungry;
			}
			
			yield return new WaitForSeconds(30f);
			
			StartCoroutine(RandomiseDefaultBehaviour());
		}
		
		#endregion

		void OnEnable()
		{
			rb = GetComponent<Rigidbody>();
			health = critterInfo.maxHealth;
			energy = critterInfo.maxEnergyLevel;
			sleepLevel = critterInfo.maxSleepLevel;
			acceleration = 15+5f/critterInfo.awakeDecayDelay;
			StartCoroutine(ComingOfAge(critterInfo.firstMatingDelay));
			StartCoroutine(RandomiseDefaultBehaviour());
			StartCoroutine(HealthRegen());
			StartCoroutine(EnergyDecay());
			StartCoroutine(SleepLevelDecay());
		}

		void OnDisable()
		{
			RemoveFromListEvent?.Invoke(transform);
		}
		
		void OnDestroy()
		{
			RemoveFromListEvent?.Invoke(transform);
		}

		public void VisionTriggerEnter(Collider other)
		{
			if (other.transform == transform) return;
			Luke.Critter go = other.GetComponent<Luke.Critter>();
			Luke.Food go2 = other.GetComponent<Luke.Food>();
			
			if (go != null)
			{
				int otherDeadliness = go.critterInfo.deadliness;
				if (otherDeadliness > critterInfo.deadliness + 3)
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
						matesList.Add(other.transform);
						go.RemoveFromListEvent += RemoveTransformFromList;
					}
				}
				else
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
		
		

		#region Blackboard Actions and Conditions
		
		public void MoveToNearestFood()
		{
			if (foodList.Count == 0) return;
			transform.LookAt(nearestFood);
			rb.AddForce(transform.TransformDirection(Vector3.forward)*acceleration);
			rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxSpeed);
		}
		
		public void MoveToNearestMate()
		{
			if (matesList.Count == 0) return;
			transform.LookAt(nearestMate);
			rb.AddForce(transform.TransformDirection(Vector3.forward)*acceleration);
			rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxSpeed);
		}
		
		public void MoveAwayFromPredator()
		{
			if (predatorsList.Count == 0) return;
			Vector3 position = transform.position;
			Vector3 heading = position - nearestPredator.position;
			transform.LookAt(position + heading);
			rb.AddForce(transform.TransformDirection(Vector3.forward)*acceleration);
			rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxSpeed);
		}

		public void LocateNearestPredator()
		{
			if (IsPredatorsListEmpty()) return;

			nearestPredator = null;
			Vector3 position = transform.position;
			float distanceToCompare = critterInfo.visionRadius;
			foreach (Transform t in predatorsList)
			{
				Vector3 translation = t.position - position;
				float distance = Vector3.Magnitude(translation);
				Physics.Raycast(new Ray(position, translation), out RaycastHit raycastHit);
				if (raycastHit.transform != t.transform) continue;
				Debug.DrawRay(position, translation, new Color(1,0,0,0.5f));
				if (!(distance < distanceToCompare)) continue;
				nearestPredator = t;
				distanceToCompare = distance;
			}

			if (nearestPredator == null) return;
				Debug.DrawLine(position, nearestPredator.position, Color.red);
		}
		
		public void LocateNearestMate()
		{
			nearestMate = null;
			Vector3 position = transform.position;
			float distanceToCompare = critterInfo.visionRadius;
			foreach (Transform t in matesList)
			{
				Vector3 translation = t.position - position;
				float distance = Vector3.Magnitude(translation);
				Physics.Raycast(new Ray(position, translation), out RaycastHit raycastHit);
				if (raycastHit.transform != t.transform) continue;
				Debug.DrawRay(position, translation, new Color (1,0,1,0.5f));
				if (!(distance < distanceToCompare)) continue;
				nearestMate = t;
				distanceToCompare = distance;
			}
			
			if (nearestMate == null) return;
			Debug.DrawLine(position, nearestMate.position, Color.magenta);
		}
		
		public void LocateNearestFood()
		{
			nearestFood = null;
			Vector3 position = transform.position;
			float distanceToCompare = critterInfo.visionRadius;
			foreach (Transform t in foodList)
			{
				Vector3 translation = t.position - position;
				float distance = Vector3.Magnitude(translation);
				Physics.Raycast(new Ray(position, translation), out RaycastHit raycastHit);
				if (raycastHit.transform != t.transform) continue;
				Debug.DrawRay(position, translation, new Color (0,1,0,0.5f));
				if (!(distance < distanceToCompare)) continue;
				
				nearestFood = t;
				distanceToCompare = distance;
			}
			
			if (nearestFood == null) return;
			Debug.DrawLine(position, nearestFood.position, Color.green);
		}
		
		public bool IsVeryTired()
		{
			return sleepLevel <= 0.25f * critterInfo.maxSleepLevel;
		}

		public bool IsTired()
		{
			return sleepLevel <= 0.5f * critterInfo.maxSleepLevel;
		}
		
		public bool IsInImminentDanger()
		{
			LocateNearestPredator();
			if (nearestPredator == null)
			{
				return false;
			}
			float distance = Vector3.Magnitude(nearestPredator.position - transform.position);
			return distance <= 0.5f * critterInfo.visionRadius;
		}

		public bool IsVeryHungry()
		{
			return energy <= 0.25f * critterInfo.maxEnergyLevel;
		}
		
		public bool IsQuiteHungry()
		{
			return energy <= 0.5f * critterInfo.maxEnergyLevel;
		}
		
		public bool IsFoodListEmpty()
		{
			return foodList.Count == 0;
		}
		
		public bool IsPredatorsListEmpty()
		{
			return predatorsList.Count == 0;
		}

		public bool IsMatesListEmpty()
		{
			return matesList.Count == 0;
		}

		public bool IsReadyToMate()
		{
			return readyToMate;
		}

		public void ReadyToMateReset()
		{
			StartCoroutine(ReadyToMateReset(critterInfo.regularMatingDelay));
		}

		public bool DefaultBehaviourIsLovelorn()
		{
			return defaultBehaviour == DefaultBehaviours.Lovelorn;
		}
		
		public bool DefaultBehaviourIsTired()
		{
			return defaultBehaviour == DefaultBehaviours.Tired;
		}
		
		public bool DefaultBehaviourIsHungry()
		{
			return defaultBehaviour == DefaultBehaviours.Hungry;
		}
		
		#endregion
	}
}
