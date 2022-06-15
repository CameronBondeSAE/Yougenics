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
    

    public float turnForce = 10f;
    public float speed = 20f;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(left))
        {
            //rb.AddRelativeTorque(0,-turnForce,0);
        }

        if (Input.GetKey(right))
        {
            //rb.AddRelativeTorque(0,turnForce,0);
        }

        if (Input.GetKey(up))
        {
            foreach (Wheel wheel in wheels)
            {
                wheel.Turn();
            }
        }

        if (Input.GetKey(down)) 
        {
            foreach (Wheel wheel in wheels)
            {
                wheel.Turn();
            }

        }
    }
}
