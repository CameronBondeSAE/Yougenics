using UnityEngine;

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

public class CreatureBase : MonoBehaviour
{
	
}

public interface IInteractable
{
	void Interact();
}