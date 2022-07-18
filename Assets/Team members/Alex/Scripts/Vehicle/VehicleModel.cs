using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleModel : MonoBehaviour, IVehicleControls
{
    public List<Wheel> wheels;
    public List<Wheel> frontWheels;
    public float turningSpeed = 50f;
    public float movementSpeed = 20f;
    public void AccelerateAndReverse(float amount)
    {
        //Moves each wheel attached to vehicle forward and backwards
        foreach (Wheel wheel in wheels)
        {
            wheel.ApplyForwardForce(amount * movementSpeed);
        }
    }
    public void Steer(float amount)
    {
        //Rotates only the front two wheels to allow vehicle to steer  
        foreach (Wheel frontWheel in frontWheels)
        {
            frontWheel.transform.localRotation = Quaternion.Euler(0, (turningSpeed * amount), 0);
        }
    }
    public Vector3 GetExitPosition()
    {
        return Vector3.zero;
    }
}
