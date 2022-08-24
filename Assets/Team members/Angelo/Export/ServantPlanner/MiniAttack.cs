using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Anthill.AI;

public class MiniAttack : AntAIState
{
    Transform child;
    public override void Create(GameObject aGameObject)
    {
        child = aGameObject.transform;
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Execute(float aDeltaTime, float aTimeScale)
    {
        Finish();
    }
}
