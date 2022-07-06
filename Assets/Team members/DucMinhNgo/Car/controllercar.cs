using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using System;
using Kev;
using Unity.VisualScripting;

namespace Minh
{
public class Controllercar : MonoBehaviour, IVehicleControls
{
    public List<Wheel> steeringWheels;

    public List<Wheel> drivingWheels;


    public float driving;
    public float speed = 1000f;
    public float steering;


    void FixedUpdate()
    {
        Control();

    }

    // Update is called once per frame
    void Control()
    {
        steering = Input.GetAxis("Horizontal") * 30f;
        foreach (Wheel steeringWheel in steeringWheels)
        {
            steeringWheel.transform.localRotation = Quaternion.Euler(0, steering, 0);
        }

        driving = Input.GetAxis("Vertical");
        foreach (Wheel drivingWheel in drivingWheels)
        {
            drivingWheel.rb.AddRelativeForce(0,0,driving * speed);
                //(driving * accelerationForce);
        }
        //driving = Input.GetAxis();
    }


    public void AccelerateAndReverse(float amount)
    {
        driving = amount;
    }

    public void Steer(float amount)
    {
        steering = amount * 30f;
    }


}

}