using System;
using System.Collections;
using System.Collections.Generic;
using Ollie;
using UnityEngine;

public class Raycastcarm : MonoBehaviour
{
    public Rigidbody rb;
    public float wheelRadius;
    public float velocity;
    public bool grounded;
    public float forceall;

    RaycastHit hitinfo;
    public void GiveForce()
    {
        rb.AddForceAtPosition(transform.up * (wheelRadius - hitinfo.distance) * forceall, transform.position);
    }

    public void FixedUpdate()
    {
        ShootRays();
    }

    public void ShootRays()
    {
        if (Physics.Raycast(transform.position, -transform.up, out hitinfo, wheelRadius))
        {
            grounded = true;
            GiveForce();
            Debug.DrawLine(transform.position, hitinfo.point, Color.green, 1f);
        }
        else
        {
            grounded = false;
            //Debug.DrawLine(transform.position, -transform.up * (travel + wheelRadius), Color.blue);
        }
        
    }
}
