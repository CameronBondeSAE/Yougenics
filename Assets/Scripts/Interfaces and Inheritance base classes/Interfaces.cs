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