using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneWing : MonoBehaviour
{

    //public float movementSpeed = 5f;

    public Rigidbody rb;
    Vector3 localVelocity;
    private float lockPos = 0;
    
    public void ApplyForwardForce(float f)
    {
        
        rb.AddForceAtPosition(transform.forward * f, transform.position, ForceMode.Force);
        
    }
    
    public void ApplySidewaysForce(float f)
    {
       
        rb.AddForceAtPosition(transform.right * f, transform.position, ForceMode.Force);
        
    }

    public void ApplyUpwardForce(float f)
    {
       
        rb.AddForceAtPosition(transform.up * f, transform.position, ForceMode.Force);
        
    }
}
