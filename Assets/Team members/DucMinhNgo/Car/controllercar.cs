using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class controllercar : MonoBehaviour
{
    public Wheel[] steeringWheels;

    public Wheel[] drivingWheels;

    public float accelerationForce;

    private float driving;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float steering = Input.GetAxis("Horizontal") * 30f;
        foreach (Wheel steeringWheel in steeringWheels)
        {
            steeringWheel.transform.localRotation = Quaternion.Euler(0, steering,0);
            
            
        }

        //driving = Input.GetAxis();
    }
}
