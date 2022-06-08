using System;
using System.Collections;
using System.Collections.Generic;
using NodeCanvas.Framework;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using Random = UnityEngine.Random;

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
		private Vector3 randomAdjustment;

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
					if (health <= 0)
					{
						Destroy(gameObject);
					}
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
			int restlessWeight = 5;

			int result = UnityEngine.Random.Range(0, lovelornWeight + tiredWeight + hungryWeight + restlessWeight);

			if (result < lovelornWeight)
			{
				defaultBehaviour = DefaultBehaviours.Lovelorn;
			}
			else if (result < lovelornWeight + tiredWeight)
			{
				defaultBehaviour = DefaultBehaviours.Tired;
			}
			else if (result < lovelornWeight + tiredWeight + hungryWeight)
			{
				defaultBehaviour = DefaultBehaviours.Hungry;
			}
			else
			{
				defaultBehaviour = DefaultBehaviours.Restless;
			}
			
			//TEMP
			bestNearbyBiome = (BestNeighbourBiome) Random.Range(0, (int)BestNeighbourBiome.LENGTH);
			Vector3 randomAdjustment = new Vector3(Random.Range(-10f, 10f), 0, Random.Range(-10f, 10f));
			
			yield return new WaitForSeconds(30f);
			
			StartCoroutine(RandomiseDefaultBehaviour());
		}

		private IEnumerator AttackCooldown()
		{
			isAttacking = true;
			yield return new WaitForSeconds(critterInfo.awakeDecayDelay);
			isAttacking = false;
		}
		
		#endregion

		/*void Update()
		{
			rb.angularVelocity = Vector3.zero;
		}*/
		
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

		private void LookAt(Vector3 target)
		{
			transform.LookAt(new Vector3 (target.x, transform.position.y, target.z));
		}

		public void Eat()
		{
			if (isSleeping) return;
			if (!Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out RaycastHit raycastHit, 1)) return;
			if (!foodList.Contains(raycastHit.collider.transform)) return;
			if (isAttacking) return;
			StartCoroutine(AttackCooldown());
			EatHealth(raycastHit.collider);
		}

		private void EatHealth(Collider target)
		{
			if (isSleeping) return;
			IEdible go = target.GetComponent<IEdible>();
			go.TakeDamage(critterInfo.deadliness);
			energy += critterInfo.deadliness;
			justAte = true;
			StopCoroutine(EnergyDecayCooldown());
			StartCoroutine(EnergyDecayCooldown());
			if (energy > critterInfo.maxEnergyLevel)
			{
				energy = critterInfo.maxEnergyLevel;
			}
		}

		public void Mate()
		{
			if (!Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out RaycastHit raycastHit, 1)) return;
			if (!foodList.Contains(raycastHit.collider.transform)) return;
			//Mate();
		}

		public void TakeDamage(float damage)
		{
			health -= damage;
			if (health > 0) return;
			Destroy(gameObject);
		}
		
		#region Blackboard Actions and Conditions
		
		public void MoveToNearestFood()
		{
			if (isSleeping) return;
			if (nearestFood == null) return;
			LookAt(nearestFood.position);
			rb.AddForce(transform.TransformDirection(Vector3.forward)*acceleration);
			rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxSpeed);
		}
		
		public void MoveToNearestMate()
		{
			if (isSleeping) return;
			if (nearestMate == null) return;
			LookAt(nearestMate.position);
			rb.AddForce(transform.TransformDirection(Vector3.forward)*acceleration);
			rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxSpeed);
		}
		
		public void MoveAwayFromPredator()
		{
			if (isSleeping) return;
			if (nearestPredator == null) return;
			Vector3 position = transform.position;
			Vector3 heading = position - nearestPredator.position;
			LookAt(position + heading);
			rb.AddForce(transform.TransformDirection(Vector3.forward)*acceleration);
			rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxSpeed);
		}

		public void MoveBiomes()
		{
			if (isSleeping) return;
			float angle = 90f * (int)bestNearbyBiome;
			//Check which way north should be or adjust bestBiome iteration to account for direction.
			Vector3 mainHeading = Quaternion.AngleAxis(angle, Vector3.up)*Vector3.forward;
			mainHeading += randomAdjustment;
			LookAt(transform.position + mainHeading);
			rb.AddForce(transform.TransformDirection(Vector3.forward)*acceleration);
			rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxSpeed);
		}

		public void IterateBiomes()
		{
			//Will change later
			//bestNearbyBiome = (BestNeighbourBiome) Random.Range(0, (int)BestNeighbourBiome.LENGTH);
		}
		
		public bool LocateNearestPredator()
		{
			if (IsPredatorsListEmpty()) return false;

			nearestPredator = null;
			Vector3 position = transform.position;
			float distanceToCompare = critterInfo.visionRadius+5f;
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

			if (nearestPredator == null) return false;
			Debug.DrawLine(position, nearestPredator.position, Color.red);
			return true;
		}
		
		public bool LocateNearestMate()
		{
			nearestMate = null;
			Vector3 position = transform.position;
			float distanceToCompare = critterInfo.visionRadius+5f;
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
			
			if (nearestMate == null) return false;
			Debug.DrawLine(position, nearestMate.position, Color.magenta);
			return true;
		}
		
		//Issues with blocked line of sight
		public bool LocateNearestFood()
		{
			nearestFood = null;
			Vector3 position = transform.position;
			float distanceToCompare = critterInfo.visionRadius+5f;
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
			
			if (nearestFood == null) return false;
			Debug.DrawLine(position, nearestFood.position, Color.green);
			return true;
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
		
		public bool DefaultBehaviourIsRestless()
		{
			return defaultBehaviour == DefaultBehaviours.Restless;
		}

		public void GoToSleep()
		{
			isSleeping = true;
		}

		public void WakeUp()
		{
			isSleeping = false;
		}

		public bool JustAte()
		{
			return justAte;
		}
		
		#endregion
	}
}
