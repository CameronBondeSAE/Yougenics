/*
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
    public VehicleModel vehicleModel;

    //public List<Wheel> rearWheels;
    // Update is called once per frame
    
    
    void Update()
    {
        if (Input.GetKey(left))
        {
            vehicleModel.Turning(-1f);
        }

        if (Input.GetKey(right))
        {
            vehicleModel.Turning(1f);
        }

        if (Input.GetKey(up))
        {
            vehicleModel.Accelerate(1f);
        }

        if (Input.GetKey(down)) 
        {
            vehicleModel.Accelerate(-1f);
        }
    }
}

*/