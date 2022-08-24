using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Anthill.AI;

public class InRangeOfAttack : AntAIState
{
    
    Transform child;
    public override void Create(GameObject aGameObject)
    {
        child = aGameObject.GetComponent<Transform>();
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Execute(float aDeltaTime, float aTimeScale)
    {
        Debug.Log("InRangeOfAttack");

        
        Finish();
        
    }
}
