using System;
using System.Collections;
using System.Collections.Generic;
using Anthill.AI;
using NodeCanvas.BehaviourTrees;
using NodeCanvas.Framework;
using ParadoxNotion;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using Random = UnityEngine.Random;

namespace Luke
{
	public class Critter : CreatureBase, IEdible, ISense
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
		public Collider currentFood;
		public Critter currentMate;
		[SerializeField]
		public BestNeighbourBiome bestNearbyBiome;
		[SerializeField]
		private Vector3 randomAdjustment;

		[SerializeField]
		private GameObject childPrefab;
		[SerializeField]
		private Transform birthingTransform;
		private Rigidbody rb;
		[SerializeField] private AStarUser aStarUser;

		private CurrentTarget currentTarget = CurrentTarget.Nothing;
		private enum CurrentTarget
		{
			Food,
			Mate,
			Predator,
			Wander,
			Nothing
		}

		#region IEnumerators

		private IEnumerator AStarReactionTime(float delay, Vector3 target)
		{
			yield return new WaitForSeconds(delay);
			aStarUser.ResetNodes(transform.position, target);
			if (aStarUser.breaker == true)
			{
				StartCoroutine(BreakerSwitch());
			}
			else aStarUser.AStarAlgorithmFast();
		}

		private IEnumerator BreakerSwitch()
		{
			yield return new WaitForSeconds(1);
			aStarUser.breaker = false;
			aStarUser.AStarAlgorithmFast();
			
		}

		public IEnumerator HealthRegen()
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

		public IEnumerator EnergyDecay()
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

		public IEnumerator EnergyDecayCooldown()
		{
			yield return new WaitForSeconds(10f);
			justAte = false;
			
			StartCoroutine(EnergyDecay());
		}
		
		public IEnumerator SleepLevelDecay()
		{
			if (isSleeping)
			{
				yield return new WaitForSeconds(critterInfo.awakeDecayDelay);
				sleepLevel += 1;
				if (sleepLevel > critterInfo.maxSleepLevel)
				{
					sleepLevel = critterInfo.maxSleepLevel;
				}
			}
			else
			{
				yield return new WaitForSeconds(critterInfo.asleepDecayDelay);
				sleepLevel -= 1;
			}

			StartCoroutine(SleepLevelDecay());
		}
		
		public IEnumerator ComingOfAge(float delay)
		{
			yield return new WaitForSeconds(delay);

			transform.Translate(Vector3.up*0.5f);
			transform.localScale = Vector3.one;
			isChild = false;
			readyToMate = true;
		}

		public IEnumerator ReadyToMateReset(float delay)
		{
			yield return new WaitForSeconds(delay);
			readyToMate = true;
		}

		// Every 30 seconds this runs a weighted randomisation for what the critter should do when not hungry or tired or near a mate when ready to mate. 
		public IEnumerator RandomiseDefaultBehaviour()
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

			yield return new WaitForSeconds(30f);
			
			StartCoroutine(RandomiseDefaultBehaviour());
		}

		public IEnumerator IterateBestBiome()
		{
			//TEMP
			bestNearbyBiome = (BestNeighbourBiome) Random.Range(0, (int)BestNeighbourBiome.LENGTH);
			randomAdjustment = new Vector3(Random.Range(-30f, 30f), 0, Random.Range(-30f, 30f));
			
			yield return new WaitForSeconds(3f);
			StartCoroutine(IterateBestBiome());
		}

		public IEnumerator AttackCooldown()
		{
			isAttacking = true;
			yield return new WaitForSeconds(critterInfo.awakeDecayDelay);
			isAttacking = false;
		}

		public IEnumerator GiveBirth(CritterInfo childInfo)
		{
			if(!Physics.Raycast(transform.position, birthingTransform.position - transform.position,
				   out RaycastHit raycastHit, Vector3.Magnitude(birthingTransform.position - transform.position)))
			{
				GameObject go = Instantiate(childPrefab);
				go.transform.position = birthingTransform.position;
				Critter childCritter = go.GetComponent<Critter>();
				childCritter.critterInfo = childInfo;
				childCritter.isChild = true;
				go.transform.localScale = Vector3.one * 0.5f;
				yield return null;
			}
			else
			{
				yield return new WaitForSeconds(1f);
				StartCoroutine(GiveBirth(childInfo));
			}
		}
		
		#endregion

		public void CollectConditions(AntAIAgent aAgent, AntAICondition aWorldState)
		{
			aWorldState.Set(LukeCritterScenario.foodNearby, !IsFoodListEmpty());
			aWorldState.Set(LukeCritterScenario.isAsleep, isSleeping);
			aWorldState.Set(LukeCritterScenario.hasFood, CheckHasFood());
			aWorldState.Set(LukeCritterScenario.hasMate, CheckHasMate());
			aWorldState.Set(LukeCritterScenario.isHealthy, IsFoodListEmpty());
			aWorldState.Set(LukeCritterScenario.isHungry, IsQuiteHungry());
			aWorldState.Set(LukeCritterScenario.isTired, IsTired());
			aWorldState.Set(LukeCritterScenario.matesNearby, !IsMatesListEmpty());
			aWorldState.Set(LukeCritterScenario.predatorsPresent, !IsPredatorsListEmpty());
			aWorldState.Set(LukeCritterScenario.isVeryHungry, IsVeryHungry());
			aWorldState.Set(LukeCritterScenario.isVeryTired, IsVeryTired());
			aWorldState.Set(LukeCritterScenario.isInImminentDanger, IsInImminentDanger());
			aWorldState.Set(LukeCritterScenario.isReadyToMate, IsReadyToMate());
			aWorldState.Set(LukeCritterScenario.DefaultBehaviourHungry, defaultBehaviour == DefaultBehaviours.Hungry);
			aWorldState.Set(LukeCritterScenario.DefaultBehaviourLovelorn, defaultBehaviour == DefaultBehaviours.Lovelorn);
			aWorldState.Set(LukeCritterScenario.DefaultBehaviourRestless, defaultBehaviour == DefaultBehaviours.Restless);
			aWorldState.Set(LukeCritterScenario.DefaultBehaviourTired, defaultBehaviour == DefaultBehaviours.Tired);
		}
		
		void OnEnable()
		{
			rb = GetComponent<Rigidbody>();
			health = critterInfo.maxHealth;
			energy = critterInfo.maxEnergyLevel;
			sleepLevel = critterInfo.maxSleepLevel;
			acceleration = 15+5f/critterInfo.awakeDecayDelay;
			regularMatingDelay = critterInfo.firstMatingDelay * 0.5f;

			foreach (Transform t in predatorsList)
			{
				t.GetComponent<Critter>().RemoveFromListEvent += RemoveTransformFromList;
			}
			foreach (Transform t in matesList)
			{
				t.GetComponent<Critter>().RemoveFromListEvent += RemoveTransformFromList;
			}
			foreach (Transform t in foodList)
			{
				t.GetComponent<IEdible>().RemoveFromListEvent += RemoveTransformFromList;
			}
			
			StartCoroutine(ComingOfAge(critterInfo.firstMatingDelay));
			StartCoroutine(RandomiseDefaultBehaviour());
			StartCoroutine(IterateBestBiome());
			StartCoroutine(HealthRegen());
			StartCoroutine(EnergyDecay());
			StartCoroutine(SleepLevelDecay());
		}

		void OnDisable()
		{
			CallRemoveEvent(transform);
		}
		
		void OnDestroy()
		{
			CallRemoveEvent(transform);
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

		private void LookAt(Vector3 target)
		{
			transform.LookAt(new Vector3 (target.x, transform.position.y, target.z));
		}

		public Vector3 GetMoveTargetAStar()
		{
			Vector3 target = transform.position;
			foreach (AStarNode node in aStarUser.path)
			{
				if (Physics.Linecast(node.worldPosition, target))
				{
					target = node.worldPosition;
					break;
				}
			}
			return target;
		}

		public bool CheckHasFood()
		{
			if (isSleeping) return false;
			if (!Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out RaycastHit raycastHit, 1)) return false;
			if (!foodList.Contains(raycastHit.collider.transform)) return false;
			if (isAttacking) return false;
			StartCoroutine(AttackCooldown());
			currentFood = raycastHit.collider;
			return true;
		}

		public void Eat()
		{
			if (isSleeping) return;
			if (currentFood == null) return;
			IEdible go = currentFood.GetComponent<IEdible>();
			go.TakeDamage(critterInfo.deadliness);
			energy += critterInfo.deadliness;
			if (energy > critterInfo.maxEnergyLevel)
			{
				energy = critterInfo.maxEnergyLevel;
			}
			justAte = true;
			StopCoroutine(EnergyDecayCooldown());
			StartCoroutine(EnergyDecayCooldown());
			if (energy > critterInfo.maxEnergyLevel)
			{
				energy = critterInfo.maxEnergyLevel;
			}
		}

		public bool CheckHasMate()
		{
			if (!Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out RaycastHit raycastHit, 1)) return false;
			
			Collider other = raycastHit.collider;
			if (!matesList.Contains(other.transform)) return false;
			
			if (critterInfo.gender == Gender.Female) return false;
			
			Critter otherScript = other.GetComponent<Critter>();
			if (!otherScript.IsReadyToMate()) return false;
			
			readyToMate = false;
			StartCoroutine(ReadyToMateReset(regularMatingDelay));
			currentMate = otherScript;
			otherScript.currentMate = this;
			otherScript.Mate();
			return true;
		}

		public void Mate()
		{
			if (critterInfo.gender == Gender.Female)
			{
				if (currentMate == null) return;
				CritterInfo currentMateInfo = currentMate.critterInfo;

				readyToMate = false;
				StartCoroutine(ReadyToMateReset(regularMatingDelay));
				
				CritterInfo childInfo = new CritterInfo();
				childInfo.maxHealth = RandomiseInfoValue(critterInfo.maxHealth, currentMateInfo.maxHealth, 50f, 300f);
				childInfo.maxSleepLevel = RandomiseInfoValue(critterInfo.maxSleepLevel, currentMateInfo.maxSleepLevel, 50f, 300f);
				childInfo.maxEnergyLevel = RandomiseInfoValue(critterInfo.maxEnergyLevel, currentMateInfo.maxEnergyLevel, 50f, 300f);
				childInfo.awakeDecayDelay = RandomiseInfoValue(critterInfo.awakeDecayDelay, currentMateInfo.awakeDecayDelay, 0.5f, 5f);
				childInfo.asleepDecayDelay = RandomiseInfoValue(critterInfo.asleepDecayDelay, currentMateInfo.asleepDecayDelay, 2f, 10f);
				childInfo.firstMatingDelay = RandomiseInfoValue(critterInfo.firstMatingDelay, currentMateInfo.firstMatingDelay, 25f, 200f);
				childInfo.isCarnivore = RandomiseInfoValue(critterInfo.isCarnivore, currentMateInfo.isCarnivore);
				childInfo.gender = RandomiseInfoValue(critterInfo.gender, currentMateInfo.gender);
				childInfo.deadliness = RandomiseInfoValue(critterInfo.deadliness, currentMateInfo.deadliness, 1, 20);
				childInfo.visionRadius = RandomiseInfoValue(critterInfo.visionRadius, currentMateInfo.visionRadius, 7f, 15f);

				StartCoroutine(GiveBirth(childInfo));
			}
		}
		
#region RandomiseInfoValue Methods

		public float RandomiseInfoValue(float selfTrait, float partnerTrait, float baseMinimum, float baseMaximum)
		{
			int determinant = Random.Range(0, 100);
			float result;
			if (determinant < 49)
			{
				result = selfTrait + selfTrait*Random.Range(-0.05f, 0.05f);
				Mathf.Clamp(result, baseMinimum, baseMaximum);
			}
			else if (determinant < 98)
			{
				result = partnerTrait + partnerTrait*Random.Range(-0.05f, 0.05f);
				Mathf.Clamp(result, baseMinimum, baseMaximum);
			}
			else
			{
				result = Random.Range(baseMinimum, baseMaximum);
			}
			return result;
		}
		
		public int RandomiseInfoValue(int selfTrait, int partnerTrait, int baseMinimum, int baseMaximum)
		{
			int determinant = Random.Range(0, 100);
			int result;
			if (determinant < 49)
			{
				result = selfTrait + Random.Range(-1, 2);
				Mathf.Clamp(result, baseMinimum, baseMaximum);
			}
			else if (determinant < 98)
			{
				result = partnerTrait + Random.Range(-1, 2);
				Mathf.Clamp(result, baseMinimum, baseMaximum);
			}
			else
			{
				result = Random.Range(baseMinimum, baseMaximum+1);
			}
			return result;
		}
		
		public bool RandomiseInfoValue(bool selfTrait, bool partnerTrait)
		{
			int determinant = Random.Range(0, 100);
			bool result;
			if (determinant < 49)
			{
				result = selfTrait;
			}
			else if (determinant < 98)
			{
				result = partnerTrait;
			}
			else
			{
				result = Random.Range(0, 2) < 0.5f;
			}
			return result;
		}
		
		private Gender RandomiseInfoValue(Gender selfTrait, Gender partnerTrait)
		{
			return Random.Range(0, 100) < 50 ? selfTrait : partnerTrait;
		}
		
#endregion

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
		
#region Blackboard Actions and Conditions
		
		public void MoveToNearestFood()
		{
			if (isSleeping) return;
			if (nearestFood == null) return;
			if (currentTarget != CurrentTarget.Food)
			{
				currentTarget = CurrentTarget.Food;
				StartCoroutine(AStarReactionTime(Random.Range(0f,1f), nearestFood.position));
			}
			LookAt(GetMoveTargetAStar());
			rb.AddForce(transform.TransformDirection(Vector3.forward)*acceleration);
			rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxSpeed);
		}
		
		public void MoveToNearestMate()
		{
			if (isSleeping) return;
			if (nearestMate == null) return;
			if (currentTarget != CurrentTarget.Mate)
			{
				currentTarget = CurrentTarget.Mate;
				StartCoroutine(AStarReactionTime(Random.Range(0f,1f), nearestMate.position));
			}
			LookAt(GetMoveTargetAStar());
			rb.AddForce(transform.TransformDirection(Vector3.forward)*acceleration);
			rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxSpeed);
		}
		
		public void MoveAwayFromPredator()
		{
			if (isSleeping) return;
			if (nearestPredator == null) return;
			currentTarget = CurrentTarget.Predator;
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
			currentTarget = CurrentTarget.Wander;
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
			float distanceToCompare = 2f*critterInfo.visionRadius;
			foreach (Transform t in predatorsList)
			{
				Vector3 targetPosition = t.position;
				Vector3 translation = targetPosition - position;
				float distance = Vector3.Magnitude(translation);
				Physics.Linecast(position, targetPosition, out RaycastHit raycastHit);
				Debug.DrawRay(position, translation, new Color(1,0,0,0.5f));
				if (raycastHit.transform != t.transform) continue;
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
				Vector3 targetPosition = t.position;
				Vector3 translation = targetPosition - position;
				float distance = Vector3.Magnitude(translation);
				Physics.Linecast(position, targetPosition, out RaycastHit raycastHit);
				Debug.DrawRay(position, translation, new Color (1,0,1,0.5f));
				if (raycastHit.transform != t.transform) continue;
				if (!(distance < distanceToCompare)) continue;
				nearestMate = t;
				distanceToCompare = distance;
			}
			
			if (nearestMate == null) return false;
			Debug.DrawLine(position, nearestMate.position, Color.magenta);
			return true;
		}
		
		public bool LocateNearestFood()
		{
			nearestFood = null;
			Vector3 position = transform.position;
			float distanceToCompare = critterInfo.visionRadius+5f;
			foreach (Transform t in foodList)
			{
				Vector3 targetPosition = t.position;
				Vector3 translation = targetPosition - position;
				float distance = Vector3.Magnitude(translation);
				Physics.Linecast(position, targetPosition, out RaycastHit raycastHit);
				Debug.DrawRay(position, translation, new Color (0,1,0,0.5f));
				if (raycastHit.transform != t.transform) continue;
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
			StartCoroutine(ReadyToMateReset(regularMatingDelay));
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

		public enum Emotions
		{
			Hungry,
			Tired,
			Mate,
			Wander,
			RunAway
		}

		public event Action<Emotions> ChangeEmotionEvent;

		public Emotions currentEmotion;

		public void ChangeEmotion(Emotions type)
		{
			currentEmotion = type;
			ChangeEmotionEvent?.Invoke(currentEmotion);
		}

		#endregion
	}
	
	public enum LukeCritterScenario
	{
		isVeryHungry = 0,
		isInImminentDanger = 1,
		isVeryTired = 2,
		isHungry = 3,
		predatorsPresent = 4,
		isReadyToMate = 5,
		isTired = 6,
		foodNearby = 7,
		matesNearby = 8,
		hasFood = 9,
		hasMate = 10,
		isHealthy = 11,
		isAsleep = 12,
		DefaultBehaviourLovelorn = 13,
		DefaultBehaviourTired = 14,
		DefaultBehaviourHungry = 15,
		DefaultBehaviourRestless = 16
	}
}
