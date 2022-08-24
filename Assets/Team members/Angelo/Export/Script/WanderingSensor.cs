using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Anthill.AI;

public class WanderingSensor : MonoBehaviour, ISense
{
    public GameObject SenseSphere;
    public float SenseRadius;
    public float AttackRadius;
    public GameObject target;

    public bool LIVE;
    public bool Engaged;
    public bool Attacking;
    public bool SeeTarget;
    public bool Wandering;
    public bool AttackerKilled;
    

    private void Start()
    {
        SenseRadius = SenseSphere.transform.localScale.x / 2;
    }

    public void Update()
    {
        target = null;
        Collider[] hitcolliders = Physics.OverlapSphere(transform.position, SenseRadius);
        foreach (var hitcollider in hitcolliders)
        {
            if (hitcollider.GetComponent<AnSensor>())
            {
                target = hitcollider.gameObject;
            }
        }
    }

    public void CollectConditions(AntAIAgent aAgent, AntAICondition aWorldState)
    {
        aWorldState.BeginUpdate(aAgent.planner);
        {
            aWorldState.Set("Wandering",Wandering);
            aWorldState.Set("SeeTarget", InRangeOfSense());
            aWorldState.Set("Engaged", Engaged);
            aWorldState.Set("Attacking",InRangeOfAttack());
            aWorldState.Set("AttackerKilled",AttackerKilled);
            aWorldState.Set("Live",LIVE);
        }
        aWorldState.EndUpdate();
    }

    public bool InRangeOfSense()
    {
        if(target != null)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool InRangeOfAttack()
    {
        if (target != null)
        {
            if (Vector3.Distance(transform.position, target.transform.position) < AttackRadius)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1, 0.5f, 0, 0.25f);
        Gizmos.DrawSphere(transform.position, SenseRadius);

        Gizmos.color = new Color(0, 1, 0, 0.35f);
        Gizmos.DrawSphere(transform.position, AttackRadius);
    }
}
