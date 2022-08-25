using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Anthill.AI;

public class Attack : AntAIState
{
    Rigidbody rb;
    WanderingSensor SenseScript;
    public float timeToPounce;
    public float turn;
    public float damp;
    public float pounceSpeed;
    public float timer;

    public float damage;
    Transform child;
    public override void Create(GameObject aGameObject)
    {
        rb = aGameObject.GetComponent<Rigidbody>();
        child = aGameObject.GetComponent<Transform>();
        SenseScript = aGameObject.GetComponent<WanderingSensor>();
    }

    public override void Enter()
    {
        timer = 0;
        base.Enter();
    }

    public override void Execute(float aDeltaTime, float aTimeScale)
    {



        Vector3 Distance = SenseScript.target.transform.position - child.transform.position;
        Vector3 DesiredRotation = Vector3.RotateTowards(child.transform.position, Distance, turn, 0);
        Debug.DrawRay(child.transform.position, DesiredRotation, Color.red);
        child.rotation = Quaternion.Slerp(child.transform.rotation, Quaternion.LookRotation(DesiredRotation), aDeltaTime * damp);

        timer += aDeltaTime;
        if(timer >= timeToPounce)
        {
            timer = 0;
            rb.AddRelativeForce(0, 0, pounceSpeed);
            if(timer>= timeToPounce + 1)
            {
                Finish();
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        
    }
}
