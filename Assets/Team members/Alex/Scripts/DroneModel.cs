using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class DroneModel : MonoBehaviour, IVehicleControls
{
    public List<DroneWing> wings;
    public List<DroneWing> frontWings;
    public List<DroneWing> backWings;
    public float turningSpeed = 50f;
    public float movementSpeed = 20f;
    

    public void AccelerateAndReverse(float amount)
    {
        foreach (DroneWing wing in wings)
        {
            wing.ApplyForwardForce(amount * movementSpeed);
        }
    }

    public void Steer(float amount)
    {
        foreach (DroneWing frontwing in frontWings)
        {
            frontwing.transform.localRotation = Quaternion.Euler(0, (turningSpeed * amount), 0);
        }
        
        foreach (DroneWing backwing in backWings)
        {
            backwing.transform.localRotation = Quaternion.Euler(0, (turningSpeed * amount * -1f), 0);
        }
    }

    public Vector3 GetExitPosition()
    {
        // TODO
        return Vector3.zero;
    }

    public void UpAndDown(float amount)
    {
        foreach (DroneWing wing in wings)
        {
            wing.ApplyUpwardForce(amount * movementSpeed);
        }
    }
}