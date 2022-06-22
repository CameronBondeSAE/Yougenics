using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleController : MonoBehaviour
{
    // Start is called before the first frame update
    public KeyCode left;
    public KeyCode right;
    public KeyCode up;
    public KeyCode down;
    public List<Wheel> wheels;
    public List<Wheel> frontWheels;
    //public List<Wheel> rearWheels;
    public float steering = 50f;
    public Rigidbody mainBodyRB; 
    public float speed = 20f;
    // Update is called once per frame
    
    
    void Update()
    {
        
        if (Input.GetKey(left))
        {
            foreach (Wheel frontWheel in frontWheels)
            {
                frontWheel.transform.localRotation = Quaternion.Euler(0, -steering, 0);
            }
        }

        if (Input.GetKey(right))
        {
            foreach (Wheel frontWheel in frontWheels)
            {
                frontWheel.transform.localRotation = Quaternion.Euler(0, steering, 0);
            }
        }
        
        

        if (Input.GetKey(up))
        {
            foreach (Wheel wheel in wheels)
            {
                mainBodyRB.AddForceAtPosition(wheel.transform.forward * speed, wheel.transform.position, ForceMode.Force);
            }
        }

        if (Input.GetKey(down)) 
        {
            foreach (Wheel wheel in wheels)
            {
                mainBodyRB.AddForceAtPosition(wheel.transform.forward * -speed, wheel.transform.position, ForceMode.Force);
            }
        }
    }
}
