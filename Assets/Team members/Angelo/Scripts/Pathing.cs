using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathing : MonoBehaviour
{
    Rigidbody rb;
    public Transform[] points;
    public GameObject parentPoints;
    public int currentTarget = 0;
    public int total;

    public float numOfPoints;
    public float radius;
    public float VisionLength = 100;
    public float pointRadius = 3;
    public float speed = 20;
    public float turn = 10;

    public float alpha;

    Vector3 velocity;
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        total = parentPoints.GetComponentsInChildren<Transform>().Length;
        total -= 1;
        points = new Transform[total];
        for (int i = 0; i < total; i++)
        {
            points[i] = parentPoints.GetComponentsInChildren<Transform>()[i+1];
        }
    }

    
    void Update()
    {
        //Vision

        //Left side Vision
        /*RaycastHit leftHit;

        if (Physics.Raycast(transform.position, transform.forward + (transform.right * radius), out leftHit, VisionLength))
        {
            float force = VisionLength - leftHit.distance;
            rb.AddRelativeTorque(0, turn * force, 0);
        }

        //Right side Vision
        RaycastHit rightHit;

        if (Physics.Raycast(transform.position, transform.forward - (transform.right * radius), out rightHit, VisionLength))
        {
            float force = VisionLength - rightHit.distance;
            rb.AddRelativeTorque(0, -turn * force, 0);
        }*/

        Collider[] hitcolliders = Physics.OverlapSphere(transform.position, pointRadius);
        foreach (var hitcollider in hitcolliders)
        {
            if(hitcollider.transform == points[currentTarget])
            {
                currentTarget += 1;
                Debug.Log("Reach Destination");
                if(currentTarget >= total)
                {
                    currentTarget = 0;
                }
            }
        }

        //Vision to target point
        RaycastHit targetHit;
        if(Physics.Linecast(transform.position, points[currentTarget].position, out targetHit))
        {
            //Debug.DrawLine(transform.position, targetHit.point);
        }
    }

    private void FixedUpdate()
    {
        Vector3 desiredVelocity = Vector3.Normalize(points[currentTarget].position - transform.position) * speed;
        Vector3 steering = desiredVelocity - rb.velocity;
        steering = Vector3limit(steering , turn);

        velocity = rb.velocity + steering;
        velocity = Vector3limit(velocity, speed);

        rb.velocity += velocity;
        
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(0, 1, 0, alpha);
        Gizmos.DrawSphere(transform.position, pointRadius);

        if(rb != null)
        {
            Gizmos.color = new Color(1, 0.2f, 0, 1);
            Gizmos.DrawLine(transform.position, velocity + transform.position);

            Gizmos.color = new Color(0, 0.2f, 1, 1);
            Gizmos.DrawLine(transform.position, rb.velocity + transform.position);
        }
    }

    private Vector3 Vector3limit(Vector3 input, float limit)
    {
        //x axis
        Vector3 temp = input;
        if (input.x > Vector3.one.x * limit)
        {
            temp.x = limit;
        }
        else if (input.x < Vector3.one.x * -limit)
        {
            temp.x = -limit;
        }

        //y axis
        if (input.y > Vector3.one.y * limit)
        {
            temp.y = limit;
        }
        else if (input.y < Vector3.one.y * -limit)
        {
            temp.y = -limit;
        }

        //z axis
        if (input.z > Vector3.one.z * limit)
        {
            temp.z = limit;
        }
        else if (input.z < Vector3.one.z * -limit)
        {
            temp.z = -limit;
        }

        return temp;
    }
}
