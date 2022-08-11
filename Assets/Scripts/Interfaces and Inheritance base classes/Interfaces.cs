using Minh;
using NodeCanvas.Tasks.Actions;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public interface IVehicleControls
{
	/// <summary>
	/// Combined forward/back
	/// </summary>
	/// <param name="amount">-1 to 1. -1 = full reverse force. 1 = full accelerate</param>
	/// <returns></returns>
	public void AccelerateAndReverse(float amount);

	/// <summary>
	/// Steering value.
	/// </summary>
	/// <param name="amount">-1 to 1. NOT angle, but total steer amount determined by individual vehicles</param>
	/// <returns></returns>
	public void Steer(float amount);

	public Vector3 GetExitPosition();
}

public interface IFlyable
{
	public void AccelerateAndReverse(float amount);
	public void StrafeLeftAndRight(float   amount);
	public void UpAndDown(float            amount);

	/// <summary>
	/// Camera vertical amount
	/// </summary>
	/// <param name="amount">0 to 1. 0 = straight down, 1 = forward</param>
	public void CameraUpAndDown(float amount);
}

public interface IEdible
{
	/// <summary>
	/// Returns total energy LEFT in this edible
	/// </summary>
	/// <returns></returns>
	float GetEnergyAmount();

	/// <summary>
	/// Call this 'per bite' from your creature. All creatures should take time to eat, not instantly
	/// </summary>
	/// <param name="energyRemoved">How much energy is ATTEMPTED to be removed per bite</param>
	/// <returns>Returns how much energy ACTUALLY got removed, eg it's been nearly fully eaten, so you TRY to remove eg 1000 energy, but there's only 10 left, so it will return 10</returns>
	float EatMe(float energyRemoved);
}

public interface IInteractable
{
	public void Interact();
}

public interface IEnergyDrainer
{
	public IEnumerator DrainCoroutine();
}


[Serializable]
public struct ItemInfo
{
	public string name;
	public string description;
	public float  energyRequired;
	public float  height;
	public float  buildTime;
}

public interface IItem
{
	void SpawnedAsNormal();

	//SpawnedAsHologram();

	/// <summary>
	///	Mostly just make your own ItemInfo variable and return it
	///		<br/>
	///		public ItemInfo GetInfo()<br/>
	///		{<br/>
	///			return itemInfo;<br/>
	///		}
	/// </summary>
	/// <returns></returns>
	ItemInfo GetInfo();
}

[Serializable]
// Generics allow TYPE to be a variable!
public class Stat<T>
{
	public string name;
	public T      Value;
}

public class CreatureBase : SerializedMonoBehaviour
{
	// public List<Stat<float>> stats;
	// public List<Stat<bool>>  statsBools;
	
	// public List<Memories> memories;

	public float age;
	public float ageOfMatingStart;
	public float ageOfMatingEnd;
	public float maxAge;
	public bool  isPregnant;
	public float gestationTime;
	public int   litterSizeMax;
	public float metabolism; // Energy efficiency and energy absorbing speed
	public float mutationRate;

	// Optional
	public float empathy;
	public float aggression;

	/// <summary>
	/// This is a percentage of the maxSize. Useful for breeding a kid that eg has half your maxsize
	/// Also mutating the maxsize in the kids is useful
	/// </summary>
	public float sizeScale;

	public float maxSize;

	public Color colour;

	Health health;

	public enum Sex
	{
		Male,
		Female
	}

	public Sex   sex;
	public float slowlyDieWhenOldRate;

	public virtual void Awake()
	{
		health = GetComponent<Health>();
	}

	public virtual void FixedUpdate()
	{
		// age
		age += Time.fixedDeltaTime;

		if (age > maxAge)
		{
			if (health != null) health.ChangeHealth(-Time.fixedDeltaTime * slowlyDieWhenOldRate);
		}
	}

	// public float Mutate(Stat selfTrait, Stat partnerTrait, Stat baseMinimum, Stat baseMaximum)
	// {
	// 	int   determinant = Random.Range(0, 100);
	// 	float result      = 0;
	// 	if (determinant < 50)
	// 	{
	// 		result = selfTrait + selfTrait*Random.Range(-0.05f, 0.05f);
	// 		Mathf.Clamp(result, baseMinimum, baseMaximum);
	// 	}
	// 	else
	// 	{
	// 		result = partnerTrait + partnerTrait*Random.Range(-0.05f, 0.05f);
	// 		Mathf.Clamp(result, baseMinimum, baseMaximum);
	// 	}
	// 		
	// 	// Random big mutation
	// 	if (determinant >= 98)
	// 	{
	// 		result += Random.Range(baseMinimum/4f, baseMaximum/4f);
	// 	}
	// 	return result;
	// }
}