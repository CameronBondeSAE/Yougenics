using System.Collections;
using System.Collections.Generic;
using NodeCanvas.Framework;
using Tanks;
using TreeEditor;
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
    public float force = 10;

    private float maxForce = 20;
    //public float minForce = 0;
    //public Vector3 OffSet;
    //public Vector3 Force;
    //public float turnForce = 10f;
    
    public float friction = 1f;

    
    void FixedUpdate()
    {
        CalculateLateralFriction();
        CalculateSuspension();
    }

    private void CalculateSuspension()
    {
        RaycastHit hitInfo;
        hitInfo = new RaycastHit();
        
        
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hitInfo, suspensionLength,
                255))
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.down) * hitInfo.distance,
                Color.yellow);
            //Debug.Log(hitInfo.distance);

            force = maxForce;
            rb.AddForceAtPosition(transform.up * force, transform.position);
        }
        else
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.down) * force, Color.white);
            //Debug.Log("Did not Hit");
            //force = maxForce;
            force = (force - hitInfo.distance);
            rb.AddForceAtPosition(transform.up * force, transform.position);
            
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

