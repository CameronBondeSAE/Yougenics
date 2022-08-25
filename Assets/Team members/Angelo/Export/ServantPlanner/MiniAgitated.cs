using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Anthill.AI;

public class MiniAgitated : AntAIState
{
    ServantSensor SenseScript;
    Transform child;
    Rigidbody rb;
    public float damp;
    public float turn;
    public float speed;
    public override void Create(GameObject aGameObject)
    {
        child = aGameObject.GetComponent<Transform>();
        SenseScript = aGameObject.GetComponent<ServantSensor>();
        rb = aGameObject.GetComponent<Rigidbody>();
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Execute(float aDeltaTime, float aTimeScale)
    {
        if (SenseScript.leaderScript.target != null)
        {
            Vector3 Distance = SenseScript.leaderScript.target.transform.position - child.transform.position;
            Vector3 DesiredRotation = Vector3.RotateTowards(child.transform.position, Distance, turn, 0);
            Debug.DrawRay(child.transform.position, DesiredRotation, Color.red);
            child.rotation = Quaternion.Slerp(child.transform.rotation, Quaternion.LookRotation(DesiredRotation), aDeltaTime * damp);
        }

        rb.AddRelativeForce(0, 0, speed);

        Finish();
    }
}
