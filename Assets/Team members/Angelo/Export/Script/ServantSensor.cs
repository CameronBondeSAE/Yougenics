using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Anthill.AI;

public class ServantSensor : MonoBehaviour, ISense
{
    public GameObject leader;
    public float attackRadius;
    public WanderingSensor leaderScript;

    public void CollectConditions(AntAIAgent aAgent, AntAICondition aWorldState)
    {
        aWorldState.BeginUpdate(aAgent.planner);
        {
            aWorldState.Set("SeeTarget", leaderSeeTarget());
            aWorldState.Set("InAttackRange", inAttackRange());
            aWorldState.Set("TrespasserKilled", false);
            aWorldState.Set("LIVE", false);
        }
        aWorldState.EndUpdate();
    }

    // Start is called before the first frame update
    void Start()
    {
        leaderScript = leader.GetComponent<WanderingSensor>();  
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private bool leaderSeeTarget()
    {
        if (leaderScript != null)
        {
            if (leaderScript.InRangeOfSense())
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

    private bool inAttackRange()
    {
        if(leaderScript != null & leaderScript.target != null)
        {
            if (Vector3.Distance(transform.position, leaderScript.target.transform.position) < attackRadius)
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
        Gizmos.color = new Color(0, 0, 1, 0.25f);
        Gizmos.DrawSphere(transform.position, attackRadius);
    }
}
