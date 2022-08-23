using System.Collections;
using System.Collections.Generic;
using Anthill.AI;
using UnityEngine;

namespace Kevin
{
    public class ISenses : CritterBAIState, ISense
    {
        public enum CritterBAI
        {
            isWandering = 0,
            isRunning = 1,
            isHunting = 2,
            isEating = 3,
            isHorny = 4,
            isMating = 5,
            isDead = 6,
            isSleeping = 7,
            isInDanger = 8,
            foundFood = 9,
            isHungry = 10,
            caughtFood = 11,
            foundMate = 12
        }

        //public CritterB parentScript;
        public VisionCritterB visionCritterB;

        public void Awake()
        {
            critterB = GetComponent<CritterB>();
        }
        public void CollectConditions(AntAIAgent aAgent, AntAICondition aWorldState)
        {
            aWorldState.Set(CritterBAI.isHungry, critterB.IsHungry());
            aWorldState.Set(CritterBAI.foundFood, critterB.FoundFood());
            aWorldState.Set(CritterBAI.isRunning, critterB.RunAway());
            aWorldState.Set(CritterBAI.isInDanger, critterB.IsInDanger());
            aWorldState.Set(CritterBAI.isSleeping, critterB.IsSleeping());
            aWorldState.Set(CritterBAI.isEating, critterB.IsEating());
            aWorldState.Set(CritterBAI.caughtFood, critterB.CaughtFood());
            aWorldState.Set(CritterBAI.isHunting, critterB.IsHunting());
            

        }
    }
    
   
}

