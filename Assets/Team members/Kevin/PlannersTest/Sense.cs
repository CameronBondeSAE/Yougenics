using System.Collections;
using System.Collections.Generic;
using Anthill.AI;
using Kev;
using UnityEngine;

namespace Kevin
{
    public class Sense : MonoBehaviour, ISense
    {
        public enum CritterAI
        {
            isSplittable = 0,
            hasFood = 1,
            isSearchingForFood = 2,
            isEating = 3,
            foodSpotted = 4,
            fullStomach = 5,
            isSplit = 6,
            isPatrolling = 7,
            caughtFood = 8
        }

        public Vision vision;
        public void CollectConditions(AntAIAgent aAgent, AntAICondition aWorldState)
        {
            /*aWorldState.Set(CritterAI.isSplittable, true);
            aWorldState.Set(CritterAI.hasFood, true);
            aWorldState.Set(CritterAI.isSearchingForFood, true);
            aWorldState.Set(CritterAI.isEating, true);
            aWorldState.Set(CritterAI.foodSpotted, true);
            aWorldState.Set(CritterAI.fullStomach, true);
            aWorldState.Set(CritterAI.isSplit, true);
            aWorldState.Set(CritterAI.isPatrolling, IAmPatrolling());
            aWorldState.Set(CritterAI.caughtFood, true);*/
            
            aWorldState.Set(CritterAI.isSearchingForFood, true);
            aWorldState.Set(CritterAI.foodSpotted, false);
            aWorldState.Set(CritterAI.isPatrolling, vision.SpottedFood());
        }
    }
}

