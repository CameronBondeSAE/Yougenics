using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Raycastcar : MonoBehaviour
{
    public float speed = 1000f;
    RaycastHit hitinfo;
    public Rigidbody rb;
    public Rigidbody car;
   


    public float suspensionLength = 1000f;
    // Start is called before the first frame update
    void Start()
    {
     
        hitinfo = new RaycastHit();
    }

    // Update is called once per frame
    void Update()
    {
        // Container for useful info coming from casting functions (note ‘out’ below)
        

        Physics.Raycast(transform.position, -transform.up, out hitinfo, suspensionLength, 255, QueryTriggerInteraction.Ignore);


// Debug: Only draw line if we hit something
        if (hitinfo.collider)
        {
            Debug.DrawLine(transform.position, hitinfo.point, Color.green);
        }

       
        rb.AddRelativeForce(0, -2f, 0);
        car.AddRelativeForce(0, 3f, 0);
        car.AddRelativeForce(0, -2f, 0);
    }


    }

