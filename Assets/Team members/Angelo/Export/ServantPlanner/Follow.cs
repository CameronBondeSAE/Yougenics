using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Anthill.AI;

public class Follow : AntAIState
{
    public GameObject pointsParent;
    Transform[] points;
    Transform child;
    Rigidbody rb;
    int total;
    int count;
    public float senseRadius;
    public float EmergencyVision;
    public float radius;
    public float turn;
    public float damp;
    public float speed;

    bool turning;
    public override void Create(GameObject aGameObject)
    {
        child = aGameObject.transform;
        rb = aGameObject.GetComponent<Rigidbody>();
        total = pointsParent.GetComponentsInChildren<Transform>().Length;
        total -= 1;
        points = new Transform[total];

        GameObject temp = aGameObject.GetComponent<ServantSensor>().leader;
        pointsParent = temp.GetComponentInChildren<SphereGizmo>().gameObject;

        for (int i = 0; i < total; i++)
        {
            points[i] = pointsParent.GetComponentsInChildren<Transform>()[i + 1];
        }
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Execute(float aDeltaTime, float aTimeScale)
    {
        turning = false;
        rb.AddRelativeForce(0, 0, speed);

        if(Vector3.Distance(points[count].position,child.transform.position) <= senseRadius)
        {
            count = Random.Range(0, total);
        }

        //Left side Vision
        RaycastHit leftHit;

        if (Physics.Raycast(transform.position, transform.forward + (transform.right * radius), out leftHit, senseRadius))
        {
            if (!leftHit.collider.GetComponent<SphereGizmo>())
            {
                float force = senseRadius - leftHit.distance;
                rb.AddRelativeTorque(0, -turn * force, 0);
                turning = true;
            }
        }

        //Right side Vision
        RaycastHit rightHit;

        if (Physics.Raycast(transform.position, transform.forward - (transform.right * radius), out rightHit, senseRadius))
        {
            if (!rightHit.collider.GetComponent<SphereGizmo>())
            {
                float force = senseRadius - rightHit.distance;
                rb.AddRelativeTorque(0, turn * force, 0);
                turning = true;
            }  
        }

        //Emergency Turn
        RaycastHit quickHit;

        if (Physics.Raycast(transform.position, transform.forward, out quickHit, EmergencyVision))
        {
            if (!quickHit.collider.GetComponent<SphereGizmo>())
            {
                rb.AddRelativeTorque(0, turn * 2, 0);
                turning = true;
            }
        }

        if (!turning)
        {
            Vector3 Distance = points[count].position - child.transform.position;
            Vector3 DesiredRotation = Vector3.RotateTowards(child.transform.position, Distance, turn, 0);
            Debug.DrawRay(child.transform.position, DesiredRotation, Color.red);
            child.rotation = Quaternion.Slerp(child.transform.rotation, Quaternion.LookRotation(DesiredRotation), aDeltaTime * damp);
        }

        Finish();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1, 0.5f, 0, 0.25f);
        Gizmos.DrawLine(transform.position, points[count].position);

        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, ((transform.forward + (transform.right * radius)) * senseRadius) + transform.position);
        Gizmos.DrawLine(transform.position, ((transform.forward - (transform.right * radius)) * senseRadius) + transform.position);
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, ((transform.forward * EmergencyVision) + transform.position));
    }
}
