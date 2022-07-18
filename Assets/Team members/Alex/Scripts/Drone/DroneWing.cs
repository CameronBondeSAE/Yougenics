using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneWing : MonoBehaviour
{

    //public float movementSpeed = 5f;

    public Rigidbody rb;
    Vector3 localVelocity;

    private float lockPos = 0;
    public bool isFlying;
    Energy energy;

    void Awake()
    {
        energy = GetComponent<Energy>();
    }
    
    private void Update()
    {
        CanFly();
    }
    

    public void ApplyForwardForce(float f)
    {
        if (isFlying)
        {
            rb.AddForceAtPosition(transform.forward * f, transform.position, ForceMode.Force);
        }
    }
    
    public void ApplySidewaysForce(float f)
    {
        if (isFlying)
        {
            rb.AddForceAtPosition(transform.right * f, transform.position, ForceMode.Force);
        }
    }

    public void ApplyUpwardForce(float f)
    {
        if (isFlying)
        {
            rb.AddForceAtPosition(transform.up * f, transform.position, ForceMode.Force);
        }
    }
    
    private void CanFly()
    {
        //If drone has energy they will ignore gravity and fly, when they run out they will fall to the ground
        if (energy.energyAmount > 0)
        {
            isFlying = true;
            rb.useGravity = false;
        }
        else
        {
            isFlying = false;
            rb.useGravity = true;
        }
    }
}
