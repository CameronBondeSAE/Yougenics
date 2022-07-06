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
        foreach (Wheel wheel in wheels)
        {
            wheel.ApplyForwardForce(amount * movementSpeed);
        }
    }

    public void Steer(float amount)
    {
        foreach (Wheel frontWheel in frontWheels)
        {
            frontWheel.transform.localRotation = Quaternion.Euler(0, (turningSpeed * amount), 0);
        }
    }
}
