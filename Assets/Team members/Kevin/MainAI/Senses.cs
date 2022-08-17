using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Anthill.AI;

namespace Kevin
{
    public class Senses : MonoBehaviour, ISense
    {
        public enum AICritterA
        {
            isHungry = 0,
            isEating = 1,
            isInDanger = 2,
            isSleeping = 3,
            canHunt = 4,
            caughtFood = 5,
            isHealthy = 6,
            readyToMate = 7,
            foundMate = 8,
            isSafe = 9,
            isMating = 10,
            enemyNearby = 11,
            isTired = 12,
            noHealth = 13,
            isDead = 14,
            WanderingBehaviour = 15,
            HungryBehaviour = 16,
            MateBehaviour = 17,
            RunningBehaviour = 18,
            SleepingBehaviour = 19,
            DeathBehaviour = 20
        }

        
        public VisionCritterA visionCritterA;

        
        
        public void CollectConditions(AntAIAgent aAgent, AntAICondition aWorldState)
        {
            /*aWorldState.Set(AICritterA.isHungry, true);
            aWorldState.Set(AICritterA.isEating, true);
            aWorldState.Set(AICritterA.isInDanger, true);
            aWorldState.Set(AICritterA.isSleeping, true);
            aWorldState.Set(AICritterA.canHunt, true);
            aWorldState.Set(AICritterA.caughtFood, true);
            aWorldState.Set(AICritterA.isHealthy, true);
            aWorldState.Set(AICritterA.readyToMate, true);
            aWorldState.Set(AICritterA.foundMate, true);
            aWorldState.Set(AICritterA.isSafe, true);
            aWorldState.Set(AICritterA.isMating, true);
            aWorldState.Set(AICritterA.enemyNearby, true);
            aWorldState.Set(AICritterA.isTired, true);
            aWorldState.Set(AICritterA.noHealth, true);
            aWorldState.Set(AICritterA.isDead, true);
            aWorldState.Set(AICritterA.WanderingBehaviour, true);
            aWorldState.Set(AICritterA.HungryBehaviour, true);
            aWorldState.Set(AICritterA.MateBehaviour, true);
            aWorldState.Set(AICritterA.RunningBehaviour, true);
            aWorldState.Set(AICritterA.SleepingBehaviour, true);
            aWorldState.Set(AICritterA.DeathBehaviour, true);*/
        }
        
        
    }
}

