using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Anthill.AI;

public class Wandering : AntAIState
{
    Transform child;
    Rigidbody rb;
    float RandomForce;
    float RandomTimer;
    float timer;
    public float VisionLength;
    public float Speed;
    public float turn;
    public float radius;
    public float EmergencyVision;
    public override void Create(GameObject aGameObject)
    {
        child = aGameObject.GetComponent<Transform>();
        rb = aGameObject.GetComponent<Rigidbody>();
        RandomForce = Random.Range(-7, 7);
        RandomTimer = Random.Range(3, 10);
    }

    public override void Enter()
    {
        
    }

    public override void Execute(float aDeltaTime, float aTimeScale)
    {
        
        timer += aDeltaTime;
        rb.AddRelativeTorque(0, RandomForce, 0);
        if (timer >= RandomTimer)
        {
            timer = 0;
            
            RandomForce = Random.Range(-7, 7);
            RandomTimer = Random.Range(3, 10);
        }


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

        Finish();
        
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
