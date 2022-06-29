using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleModel : MonoBehaviour
{
    public List<Wheel> wheels;
    public List<Wheel> frontWheels;
    public float turningSpeed = 50f;
    public float movementSpeed = 20f;
    public bool isSteering;
    public float returnToZero = .1f; 


    public void FixedUpdate()
    {
        foreach (Wheel frontWheel in frontWheels)
        {
            frontWheel.transform.localRotation = Quaternion.Lerp(frontWheel.transform.localRotation, Quaternion.Euler(0,0,0) , returnToZero);
        }
    }
    
    public void Turning(float f)
    {

            foreach (Wheel frontWheel in frontWheels)
            {
                frontWheel.transform.localRotation = Quaternion.Euler(0, (turningSpeed * f), 0);
            }
        
    }


    public void Accelerate(float f)
    {
        foreach (Wheel wheel in wheels)
        {
            wheel.ApplyForwardForce(f * movementSpeed);
        }
    }
}
