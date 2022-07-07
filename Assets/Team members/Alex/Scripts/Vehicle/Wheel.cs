using System.Collections;
using System.Collections.Generic;
using NodeCanvas.Framework;
using Tanks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class Wheel : MonoBehaviour
{

    //public float movementSpeed = 5f;

    public Rigidbody rb;
    //Vector3 movement;
    
    Vector3 localVelocity;
    public float suspensionLength = 3f;
    private float force = 100;
    private float maxHeight = 1f;
    public bool onGround;
    private float maxForce = 200;
    private float friction = 1f;
    public float suspensionForce = 100f;
    
    void FixedUpdate()
    {
        CalculateLateralFriction();
        CalculateSuspension();
    }

    private void CalculateSuspension()
    {
        RaycastHit hitInfo;
        hitInfo = new RaycastHit();
        


        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hitInfo,
                suspensionLength,
                255))
        {
            float heightOffGround = hitInfo.distance;
            force = maxHeight - heightOffGround;

            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.down) * hitInfo.distance,
                Color.yellow);


            rb.AddForceAtPosition(transform.up * (force * suspensionForce), transform.position);
            onGround = true;
        }
        else
        {
            onGround = false;
        }
    }

    public void ApplyForwardForce(float f)
    {
        if (onGround)
        {
            rb.AddForceAtPosition(transform.forward * f, transform.position, ForceMode.Force);
        }
    }


    private void CalculateLateralFriction()
    {
        localVelocity = transform.InverseTransformDirection(rb.velocity);
        
        rb.AddRelativeForce(-localVelocity.x * (rb.mass/friction), 0, 0);
    }

    /*
    private void CalculateLongitudinalFriction()
    {
        localVelocity = transform.InverseTransformDirection(rb.velocity);
        
        rb.AddRelativeForce(0, -localVelocity.x * friction, 0);
    }
    */

    
}

