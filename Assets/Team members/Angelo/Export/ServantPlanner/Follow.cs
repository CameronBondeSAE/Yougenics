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
    public float turn;
    public float damp;
    public float speed;
    public override void Create(GameObject aGameObject)
    {
        child = aGameObject.transform;
        rb = aGameObject.GetComponent<Rigidbody>();
        total = pointsParent.GetComponentsInChildren<Transform>().Length;
        total -= 1;
        points = new Transform[total];
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

        Vector3 Distance = points[count].position - child.transform.position;
        Vector3 DesiredRotation = Vector3.RotateTowards(child.transform.position, Distance, turn, 0);
        Debug.DrawRay(child.transform.position, DesiredRotation, Color.red);
        child.rotation = Quaternion.Slerp(child.transform.rotation, Quaternion.LookRotation(DesiredRotation), aDeltaTime * damp);
        
        rb.AddRelativeForce(0, 0, speed);

        if(Vector3.Distance(points[count].position,child.transform.position) <= senseRadius)
        {
            count = Random.Range(0, total);
        }

        Finish();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1, 0.5f, 0, 0.25f);
        Gizmos.DrawLine(transform.position, points[count].position);
    }
}
