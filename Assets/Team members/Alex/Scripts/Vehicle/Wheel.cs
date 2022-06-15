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
    public float force = 20f;
    //public float minForce = 0;
    public Vector3 OffSet;
    public Vector3 Force;
    public float turnForce = 10f;
    public float speed = 20f;
    


    // Start is called before the first frame update
    void Start()
    {

    }
    
    public void Turn()
    {
        rb.AddForceAtPosition(transform.forward * speed, transform.TransformPoint(OffSet));
        Debug.Log("MoveTest");
        
    }

        // Update is called once per frame
    void Update()
    {
        RaycastHit hitInfo;
        hitInfo = new RaycastHit();
        localVelocity = transform.InverseTransformDirection(rb.velocity);
        //Debug.Log(localVelocity);
        //rb.transform.position = OffSet;

        rb.AddForceAtPosition(transform.up, transform.TransformPoint(OffSet));

        if (localVelocity.x > 0)
        {
            rb.AddRelativeForce(-1, 0, 0);
        }
        if (localVelocity.x < 0)
        {
            rb.AddRelativeForce(1, 0, 0);
        }
        
        

        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hitInfo, suspensionLength, 255))
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.down) * hitInfo.distance,
                Color.yellow);
            
            //force = maxForce - layerMask;
            Debug.Log("Did Hit");
            rb.AddRelativeForce(0,force,0 );
        }
        else
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.down) * force, Color.white);
            Debug.Log("Did not Hit");
            //force = maxForce;
            rb.AddRelativeForce(0,0,0 );
        }
    }
}

