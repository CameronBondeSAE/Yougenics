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
    //public List<FrontWheels> frontWheels;
    //public List<RearWheels> rearWheels;
    public float steering = 20f;
    

    public float turnForce = 10f;
    public float speed = 20f;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(left))
        {
            //steeringWheel.transform.localRotation = Quaternion.Euler(0, steering, 0);
            foreach (Wheel wheel in wheels)
            {
                wheel.transform.localRotation = Quaternion.Euler(0, steering * -1, 0);
            }
        }

        if (Input.GetKey(right))
        {
            //steeringWheel.transform.localRotation = Quaternion.Euler(0, steering * -1, 0);
            foreach (Wheel wheel in wheels)
            {
                wheel.transform.localRotation = Quaternion.Euler(0, steering, 0);
            }
        }

        if (Input.GetKey(up))
        {
            foreach (Wheel wheel in wheels)
            {
                wheel.Forward();
            }
        }

        if (Input.GetKey(down)) 
        {
            foreach (Wheel wheel in wheels)
            {
                wheel.Back();
            }

        }
    }
}
