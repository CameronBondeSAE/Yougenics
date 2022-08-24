using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Steering : MonoBehaviour
{
    public float VisionLength;
    public float Speed;
    public float turn;
    public float radius;
    public float EmergencyVision;
    Rigidbody rb;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
   
    void Update()
    {
        //Walking
        rb.AddRelativeForce(0, 0, Speed);


        //Vision

        //Left side Vision
        RaycastHit leftHit;

        if (Physics.Raycast(transform.position, transform.forward + (transform.right * radius), out leftHit, VisionLength))
        {
            float force = VisionLength - leftHit.distance;
            rb.AddRelativeTorque(0, -turn * force, 0);
            
        }

        //Right side Vision
        RaycastHit rightHit;

        if (Physics.Raycast(transform.position, transform.forward - (transform.right * radius), out rightHit, VisionLength))
        {
            float force = VisionLength - rightHit.distance;
            rb.AddRelativeTorque(0, turn * force, 0);
        }

        //Emergency Turn
        RaycastHit quickHit;

        if (Physics.Raycast(transform.position, transform.forward, out quickHit, EmergencyVision))
        {
            rb.AddRelativeTorque(0, turn * 2, 0);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, ((transform.forward + (transform.right * radius)) * VisionLength) + transform.position);
        Gizmos.DrawLine(transform.position, ((transform.forward - (transform.right * radius)) * VisionLength) + transform.position);
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, ((transform.forward * EmergencyVision) + transform.position));
    }
}
