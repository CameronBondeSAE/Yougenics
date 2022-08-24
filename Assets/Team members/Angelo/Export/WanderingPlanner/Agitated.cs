using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Anthill.AI;

public class Agitated : AntAIState
{
    WanderingSensor SenseScript;
    Transform child;
    Rigidbody rb;
    public float damp;
    public float turn;
    public float speed;
    Vector3 velocity;
    public override void Create(GameObject aGameObject)
    {
        child = aGameObject.GetComponent<Transform>();
        SenseScript = aGameObject.GetComponent<WanderingSensor>();
        rb = aGameObject.GetComponent<Rigidbody>();
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Execute(float aDeltaTime, float aTimeScale)
    {
        Debug.Log("Agitated");

        
        if(SenseScript.target != null)
        {
            Vector3 Distance = SenseScript.target.transform.position - child.transform.position;
            Vector3 DesiredRotation = Vector3.RotateTowards(child.transform.position, Distance, turn, 0);
            Debug.DrawRay(child.transform.position, DesiredRotation, Color.red);
            child.rotation = Quaternion.Slerp(child.transform.rotation, Quaternion.LookRotation(DesiredRotation), aDeltaTime * damp);
        }

        rb.AddRelativeForce(0, 0, speed);

        //Vector3 steering = desiredVelocity - rb.velocity;
        //steering = Vector3limit(steering, turn);


        //velocity = rb.velocity + steering;
        //velocity = Vector3limit(velocity, speed);

        //rb.velocity += velocity;

        Finish();
        
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

    private void OnDrawGizmos()
    {
        
    }
}
