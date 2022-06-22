using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayCastTest : MonoBehaviour
{
    public Rigidbody rb;
    public void Update()
    {
        RaycastHit hitInfo;
        hitInfo = new RaycastHit();

        int layerMask = 1 << 8;
        layerMask = ~layerMask;


        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hitInfo, 3,
                layerMask))
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.down) * hitInfo.distance,
                Color.yellow);

            //force = maxForce - layerMask;
            Debug.Log("Did Hit");
            rb.AddRelativeForce(0, 1, 0);
        }
        else
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.down) * 3, Color.white);
            Debug.Log("Did not Hit");
            //force = maxForce;
            rb.AddRelativeForce(0, 0, 0);
        }
    }
}
