using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using Anthill.AI;
using Cam;
using DG.Tweening;
using Maya;
using NodeCanvas.BehaviourTrees;
using NodeCanvas.Framework;
using ParadoxNotion;
using Unity.Mathematics;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using Random = UnityEngine.Random;

namespace Luke
{
	public class Critter : CreatureBase, IEdible, ISense, ILukeEdible
	{
		public CritterInfo critterInfo;
		
		public DefaultBehaviours defaultBehaviour;
		public float sleepLevel;
		public float acceleration;
		public float turnSpeed = 3f;
		public float maxSpeed;
		public float awakeDecayDelay;
		public float regularMatingDelay;
		public bool cannotMate = true;
		public bool readyToMate = false;
		public bool isSleeping = false;
		public bool justAte = false;
		public bool isAttacking = false;
		public bool canWakeUp = true;
		public float attackRange = 2f;

		private bool turnBiasRight;

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
		private int pathProgress = 1;
		
		[SerializeField]
		private PrefabReferenceHack childPrefab;
		[SerializeField]
		private Transform birthingTransform;
		private Rigidbody _rb;
		private Transform _transform;
        [SerializeField] private Minh.Health healthComp;
        [SerializeField] private Energy energyComp;
        [SerializeField] private CommonAttributes _commonAttributes;

        //spawn these in and give them angles
        [SerializeField] private List<LukeAntenna> antennae = new();
        public int numberOfAntennae;
        public GameObject antennaPrefab;
        public GameObject bumperPrefab;
        [SerializeField] private GameObject beam;
        
        [SerializeField]
		private Transform view;
		[SerializeField] private AStarUser aStarUser;

		private CurrentTarget _currentTarget = CurrentTarget.Nothing;
		private enum CurrentTarget
		{
			Food,
			Mate,
			Predator,
			Wander,
			Nothing
		}

		#region IEnumerators

		private IEnumerator SleepDebt()
		{
			yield return new WaitForSeconds(3f);
			canWakeUp = true;
		}

		private IEnumerator ChangeTurnBias()
		{
			yield return new WaitForSeconds(4f);
			turnBiasRight = !turnBiasRight;
			StartCoroutine(ChangeTurnBias());
		}
		
		private IEnumerator AStarReactionTime(float delay, Vector3 target)
		{
			yield return new WaitForSeconds(delay);
			aStarUser.ResetNodes(transform.position, target);
			if (aStarUser.breaker == true)
			{
				StartCoroutine(BreakerSwitch());
			}
			else aStarUser.BeginAStarAlgorithm();
		}

		private IEnumerator BreakerSwitch()
		{
			yield return new WaitForSeconds(1);
			aStarUser.breaker = false;
			aStarUser.BeginAStarAlgorithm();
			
		}

        private IEnumerator EnergyDecayCooldown()
		{
            // change energy drain amount, then return it after yield
			yield return new WaitForSeconds(10f);
			justAte = false;
            
		}
		
		private IEnumerator SleepLevelDecay()
		{
			if (isSleeping)
			{
				yield return new WaitForSeconds(awakeDecayDelay);
				sleepLevel += 1;
				if (sleepLevel > critterInfo.maxSleepLevel)
				{
					sleepLevel = critterInfo.maxSleepLevel;
				}
			}
			else
			{
				yield return new WaitForSeconds(awakeDecayDelay*2);
				sleepLevel -= 1;
			}

			StartCoroutine(SleepLevelDecay());
		}

		private IEnumerator ComingOfAge(float delay)
		{
			yield return new WaitForSeconds(delay);

			//Do view stuff
			//comingOfAgeEvent?.Invoke();
			ComingOfAgeViewHackClientRpc();

			cannotMate = false;
			readyToMate = true;
		}

		[ClientRpc]
		public void ComingOfAgeViewHackClientRpc()
		{
			transform.Translate(Vector3.up * 0.5f);
			transform.DOPunchScale(transform.localScale * 1.5f, 0.5f);
		}

		private IEnumerator EndOfMatingAge(float delay)
		{
			yield return new WaitForSeconds(delay);
			cannotMate = true;
			readyToMate = false;
		}

		private IEnumerator ReadyToMateReset(float delay)
		{
			yield return new WaitForSeconds(delay);
			readyToMate = true;
		}

		// Every 30 seconds this runs a weighted randomisation for what the critter should do when not hungry or tired or near a mate when ready to mate. 
		private IEnumerator RandomiseDefaultBehaviour()
		{
			int lovelornWeight = readyToMate ? 3 : 0;
			int tiredWeight = Mathf.RoundToInt(5 * (sleepLevel / critterInfo.maxSleepLevel));
			int hungryWeight = Mathf.RoundToInt(5 * (energyComp.EnergyAmount.Value / energyComp.energyMax));
			int restlessWeight = 5;

			int result = Random.Range(0, lovelornWeight + tiredWeight + hungryWeight + restlessWeight);

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

		private IEnumerator IterateBestBiome()
		{
			//TEMP
			bestNearbyBiome = (BestNeighbourBiome) Random.Range(0, (int)BestNeighbourBiome.LENGTH);
			randomAdjustment = new Vector3(Random.Range(-30f, 30f), 0, Random.Range(-30f, 30f));
			
			yield return new WaitForSeconds(2f);
			StartCoroutine(IterateBestBiome());
		}

		private IEnumerator AttackCooldown()
		{
			isAttacking = true;
			yield return new WaitForSeconds(awakeDecayDelay);
			isAttacking = false;
		}

		private IEnumerator BePregnant(List<CritterInfo> childrenInfo, float delay)
		{
			isPregnant = true;
			
			yield return new WaitForSeconds(delay);
			
			StartCoroutine(GiveBirth(childrenInfo, 0));
		}

		private IEnumerator GiveBirth(List<CritterInfo> childrenInfo, int childIterator)
		{
			if (childIterator < litterSizeMax)
			{
				if (!Physics.Raycast(transform.position, birthingTransform.position - transform.position,
					    out RaycastHit raycastHit, Vector3.Magnitude(birthingTransform.position - transform.position)))
				{
					GameObject go = Instantiate(childPrefab.prefab);
					go.transform.position = birthingTransform.position;
					Critter childCritter = go.GetComponent<Critter>();
					childCritter.critterInfo = childrenInfo[childIterator];
					childCritter.cannotMate = true;
					isPregnant = false;
					childIterator++;
				}
				yield return new WaitForSeconds(1f);
				StartCoroutine(GiveBirth(childrenInfo, childIterator));
			}
			else
			{
				yield return null;
			}
		}
		
		#endregion

		void Start()
		{
            _rb = GetComponent<Rigidbody>();
			_transform = GetComponent<Transform>();
			SetInitialCritterStats();
			sleepLevel = critterInfo.maxSleepLevel;
			acceleration = Mathf.Clamp(20+10f/metabolism,20,30);
			regularMatingDelay = ageOfMatingStart * 0.5f;

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
				t.GetComponent<Critter>().RemoveFromListEvent += RemoveTransformFromList;
				t.GetComponent<Food>().RemoveFromListEvent += RemoveTransformFromList;
			}
			
			StartCoroutine(ComingOfAge(ageOfMatingStart));
			StartCoroutine(EndOfMatingAge(ageOfMatingEnd));
			StartCoroutine(RandomiseDefaultBehaviour());
			StartCoroutine(IterateBestBiome());
			StartCoroutine(SleepLevelDecay());
			StartCoroutine(ChangeTurnBias());

			GameObject go1 = Instantiate(bumperPrefab, _transform);
			LukeBumper bumper = go1.GetComponent<LukeBumper>();
            for (int i = 0; i < numberOfAntennae; i++)
            {
                GameObject go = Instantiate(antennaPrefab, _transform);
                go.transform.localPosition = new Vector3(0, -_transform.localScale.y/4f, 0);
                go.transform.localEulerAngles = new Vector3(0, -45+90*i/(numberOfAntennae-1), 0);
                bumper.antennae.Add(go);
            }
		}

		public override void FixedUpdate()
		{
			base.FixedUpdate();
			_transform.rotation = Quaternion.Euler(new Vector3(0, _transform.rotation.eulerAngles.y, 0));
			SetScale();
		}

		void OnDisable()
		{
			CallRemoveEvent(_transform);
		}
		
		void OnDestroy()
		{
			CallRemoveEvent(_transform);
		}

		public void VisionTriggerEnter(Collider other)
		{
            //Change once allegiances are implemented
			if (other.transform == _transform) return;
			CreatureBase go1 = other.GetComponentInParent<CreatureBase>();
			IEdible go2 = other.GetComponentInParent<IEdible>();
			CommonAttributes go3 = other.GetComponentInParent<CommonAttributes>(); //Nulling out
			ILukeEdible go4 = other.GetComponentInParent<ILukeEdible>();
			
			if (go1 != null && go3 != null)
			{
				float otherDangerLevel = go3.dangerLevel;
				if (otherDangerLevel >= critterInfo.dangerLevel + 3)
				{
					if (!predatorsList.Contains(go1.transform))
					{
						predatorsList.Add(go1.transform);
						if (go4 != null) go4.RemoveFromListEvent += RemoveTransformFromList;
					}
				}
				else if (otherDangerLevel < critterInfo.dangerLevel+3 && otherDangerLevel > critterInfo.dangerLevel-3)
				{
					if (!matesList.Contains(go1.transform))
					{
						Critter otherCritter = other.GetComponentInParent<Critter>();
						if (otherCritter != null && critterInfo.gender != otherCritter.critterInfo.gender && !otherCritter.cannotMate)
						{
							matesList.Add(otherCritter.transform);
                            if (go4 != null) go4.RemoveFromListEvent += RemoveTransformFromList;
						}
					}
				}
				else if (critterInfo.isCarnivore)
				{
					if (!foodList.Contains(go1.transform))
					{
						foodList.Add(go1.transform);
                        if (go4 != null) go4.RemoveFromListEvent += RemoveTransformFromList;
					}
				}
			}
			else if (go2 != null)
			{
				if (!foodList.Contains(((MonoBehaviour)go2).transform))
				{
					foodList.Add(((MonoBehaviour)go2).transform);
                    if (go4 != null) go4.RemoveFromListEvent += RemoveTransformFromList;
				}
			}
		}
		
		public void VisionTriggerExit(Collider other)
		{
			RemoveTransformFromList(other.transform);
		}

		private void RemoveTransformFromList(Transform trans)
		{
			if (predatorsList.Contains(trans))
			{
				predatorsList.Remove(trans);
			}
			if (matesList.Contains(trans))
			{
				matesList.Remove(trans);
			}
			if (foodList.Contains(trans))
			{
				foodList.Remove(trans);
			}
		}

		private void SetScale()
		{
			//include eating in equation
			/*float scale = Mathf.Min(0.5f+(maxSize-0.5f) * age / maxAge,maxSize);
			view.localScale = Vector3.one*scale;*/

			SetScaleViewHackClientRpc();
		}

		[ClientRpc]
		public void SetScaleViewHackClientRpc()
		{
			float scale = Mathf.Min(0.5f + (maxSize - 0.5f) * age / maxAge, maxSize);
			transform.localScale = Vector3.one * scale;
		}

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
			aWorldState.Set(LukeCritterScenario.canWakeUp, canWakeUp);
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
		
		/*public int RandomiseInfoValue(int selfTrait, int partnerTrait, int baseMinimum, int baseMaximum)
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
		}*/
		
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
		
		private Sex RandomiseInfoValue(Sex selfTrait, Sex partnerTrait)
		{
			return Random.Range(0, 100) < 50 ? selfTrait : partnerTrait;
		}
		
		#endregion
		
		#region Blackboard Actions and Conditions
		
		private void SetInitialCritterStats()
		{
			_commonAttributes.dangerLevel = critterInfo.dangerLevel;
            energyComp.drainSpeed = 11-metabolism;
			critterInfo.gender = sex;
            ageOfMatingStart = critterInfo.firstMatingDelay;
            energyComp.energyMax = critterInfo.maxEnergyLevel;
            healthComp.maxHealth = critterInfo.maxHealth;
            healthComp.ChangeHealth(healthComp.maxHealth);
            energyComp.ChangeEnergy(energyComp.energyMax);
        }
		
		private void TurnTowardsTarget(Vector3 target)
		{
			float angle = Vector3.SignedAngle(_transform.forward, target - _transform.position, Vector3.up);
			_rb.AddRelativeTorque(new Vector3(0,Mathf.Sign(angle)*turnSpeed,0));
		}

		public Vector3 GetMoveTargetAStar()
		{
			if (Vector3.Distance(_transform.position,
				    aStarUser.aStarManager.Nodes[aStarUser.path[^pathProgress].x, aStarUser.path[^pathProgress].y]
					    .worldPosition) > 1)
			{
				pathProgress++;
			}
			return aStarUser.aStarManager.Nodes[aStarUser.path[^pathProgress].x,aStarUser.path[^pathProgress].y].worldPosition;
        }

		public bool CheckHasFood()
        {
            if (_transform == null) return false;
			if (isSleeping) return false;
			if (!Physics.Raycast(_transform.position, _transform.forward*attackRange, out RaycastHit raycastHit, 3)) return false;
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
            GameObject go = Instantiate(beam, _transform);
            go.GetComponent<Beam>().target = currentFood.transform;
			Minh.Health targetHealth = currentFood.GetComponent<Minh.Health>();
            targetHealth.ChangeHealth(-critterInfo.dangerLevel);
            IEdible targetIEdible = currentFood.GetComponent<IEdible>();
            energyComp.ChangeEnergy(targetIEdible.EatMe(-critterInfo.dangerLevel));
            energyComp.ChangeEnergy(critterInfo.dangerLevel);
            justAte = true;
			StopCoroutine(EnergyDecayCooldown());
			StartCoroutine(EnergyDecayCooldown());
        }

		public bool CheckHasMate()
		{
            if (_transform == null) return false;
			if (!Physics.Raycast(_transform.position, _transform.TransformDirection(Vector3.forward), out RaycastHit raycastHit, 1)) return false;
			
			Collider other = raycastHit.collider;
			if (!matesList.Contains(other.transform)) return false;
			
			if (critterInfo.gender == Sex.Female) return false;
			
			Critter otherScript = other.GetComponent<Critter>();
			if (!otherScript.IsReadyToMate() || otherScript.isPregnant) return false;
			
			readyToMate = false;
			StartCoroutine(ReadyToMateReset(regularMatingDelay));
			currentMate = otherScript;
			otherScript.currentMate = this;
			otherScript.Mate();
			return true;
		}

		public void Mate()
		{
			if (critterInfo.gender == Sex.Female)
			{
				if (currentMate == null) return;
				CritterInfo currentMateInfo = currentMate.critterInfo;

				readyToMate = false;
				StartCoroutine(ReadyToMateReset(regularMatingDelay));

				List<CritterInfo> childrenInfo = new(litterSizeMax);

				for (int i = 0; i < litterSizeMax; i++)
				{
					CritterInfo child = new()
					{
						maxHealth = RandomiseInfoValue(critterInfo.maxHealth, currentMateInfo.maxHealth, 50f, 300f),
						maxSleepLevel = RandomiseInfoValue(critterInfo.maxSleepLevel, currentMateInfo.maxSleepLevel,
							50f, 300f),
						maxEnergyLevel = RandomiseInfoValue(critterInfo.maxEnergyLevel, currentMateInfo.maxEnergyLevel,
							50f, 300f),
						metabolism = RandomiseInfoValue(critterInfo.metabolism,
							currentMateInfo.metabolism, 0.5f, 10f),
						firstMatingDelay = RandomiseInfoValue(critterInfo.firstMatingDelay,
							currentMateInfo.firstMatingDelay, 25f, 200f),
						isCarnivore = RandomiseInfoValue(critterInfo.isCarnivore, currentMateInfo.isCarnivore),
						gender = RandomiseInfoValue(critterInfo.gender, currentMateInfo.gender),
						dangerLevel = RandomiseInfoValue(critterInfo.dangerLevel, currentMateInfo.dangerLevel, 1, 20),
						visionRadius = RandomiseInfoValue(critterInfo.visionRadius, currentMateInfo.visionRadius, 7f,
							15f)
					};
					childrenInfo.Add(child);
				}

				StartCoroutine(BePregnant(childrenInfo, gestationTime));
			}
		}
		
        //Does this need to be in an interface?
		public void TakeDamage(float damage)
		{
            healthComp.ChangeHealth(-damage);
		}

        public void Death()
        {
            //code me
        }
        
        public event ILukeEdible.RemoveFromListAction RemoveFromListEvent;

        public void CallRemoveEvent(Transform trans)
		{
			RemoveFromListEvent?.Invoke(trans);
		}

		public void MoveToNearestFood()
		{
			if (isSleeping) return;
			if (nearestFood == null) return;
			/*if (_currentTarget != CurrentTarget.Food)
			{
				_currentTarget = CurrentTarget.Food;
				StartCoroutine(AStarReactionTime(Random.Range(0f,1f), nearestFood.position));
			}*/

			TurnTowardsTarget(nearestFood.position);
			
			/*if (aStarUser.path.Count > 5)
			{
				TurnTowardsTarget(GetMoveTargetAStar());
                Debug.Log(GetMoveTargetAStar());
			}
			else
			{
				TurnTowardsTarget(nearestFood.position);
			}*/
			_rb.AddForce(_transform.forward*acceleration, ForceMode.Acceleration);
			_rb.velocity = Vector3.ClampMagnitude(_rb.velocity, maxSpeed);
		}
		
		public void MoveToNearestMate()
		{
			if (isSleeping) return;
			if (nearestMate == null) return;
			/*if (_currentTarget != CurrentTarget.Mate)
			{
				_currentTarget = CurrentTarget.Mate;
				StartCoroutine(AStarReactionTime(Random.Range(0f,1f), nearestMate.position));
			}*/
			
			TurnTowardsTarget(nearestMate.position);
			
			/*if (aStarUser.path.Count > 5)
			{
				TurnTowardsTarget(GetMoveTargetAStar());
			}
			else
			{
				TurnTowardsTarget(nearestMate.position);
			}*/
			_rb.AddForce(_transform.TransformDirection(Vector3.forward)*acceleration, ForceMode.Acceleration);
			_rb.velocity = Vector3.ClampMagnitude(_rb.velocity, maxSpeed);
		}
		
		public void MoveAwayFromPredator()
		{
			if (isSleeping) return;
			if (nearestPredator == null) return;
			_currentTarget = CurrentTarget.Predator;
			Vector3 position = _transform.position;
			Vector3 heading = position - nearestPredator.position;
			TurnTowardsTarget(position + heading);
			_rb.AddForce(_transform.TransformDirection(Vector3.forward)*acceleration, ForceMode.Acceleration);
			_rb.velocity = Vector3.ClampMagnitude(_rb.velocity, maxSpeed);
		}

		public void MoveBiomes()
		{
			if (isSleeping) return;
			float angle = 90f * (int)bestNearbyBiome;
			//Check which way north should be or adjust bestBiome iteration to account for direction.
			Vector3 mainHeading = Quaternion.AngleAxis(angle, Vector3.up)*Vector3.forward;
			mainHeading += randomAdjustment;
			_currentTarget = CurrentTarget.Wander;
			TurnTowardsTarget(_transform.position + mainHeading);
			_rb.AddForce(_transform.TransformDirection(Vector3.forward)*acceleration, ForceMode.Acceleration);
			_rb.velocity = Vector3.ClampMagnitude(_rb.velocity, maxSpeed);
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
			Vector3 position = _transform.position;
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
			Vector3 position = _transform.position;
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
			Vector3 position = _transform.position;
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
			float distance = Vector3.Magnitude(nearestPredator.position - _transform.position);
			return distance <= 0.5f * critterInfo.visionRadius;
		}

		public bool IsVeryHungry()
		{
			return energyComp.EnergyAmount.Value <= 0.25f * energyComp.energyMax;
		}
		
		public bool IsQuiteHungry()
		{
			return energyComp.EnergyAmount.Value <= 0.5f * energyComp.energyMax;
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
			canWakeUp = false;
			StartCoroutine(SleepDebt());
		}

		public void WakeUp()
		{
			isSleeping = false;
			canWakeUp = true;
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

        public float GetEnergyAmount()
        {
            return energyComp.EnergyAmount.Value;
        }

        public event Action TakeDamageEvent;
        
        public float EatMe(float energyRemoved)
        {
	        TakeDamageEvent?.Invoke();
	        if (energyComp.EnergyAmount.Value > energyRemoved) return energyRemoved;
	        return energyComp.EnergyAmount.Value;
        }
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
		DefaultBehaviourRestless = 16,
		canWakeUp = 17
	}
}
