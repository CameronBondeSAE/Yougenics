using Anthill.AI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnSensor : MonoBehaviour, ISense
{
    public AnVision Vision;
    public bool Alive;
    public bool Patrolling;
    public bool Hunting;
    public bool Hungry;
    public bool TargetHunted;
    public void CollectConditions(AntAIAgent aAgent, AntAICondition aWorldState)
    {
        aWorldState.BeginUpdate(aAgent.planner);
        {
            aWorldState.Set("Alive", Alive);
            aWorldState.Set("Patroling", Patrolling);
            aWorldState.Set("SeeTarget", Vision.Refresh());
            aWorldState.Set("Hunting", Hunting);
            aWorldState.Set("Hungry", Hungry);
            aWorldState.Set("TargetHunted", TargetHunted);
        }
        aWorldState.EndUpdate();
        
    }
}
