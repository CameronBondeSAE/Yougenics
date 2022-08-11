using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Anthill.AI;

namespace Kevin
{
    public class Senses : MonoBehaviour, ISense
    {
        public enum CritterA
        {
            isSplittable = 0,
            hasFood = 1,
            isSearchingForFood = 2,
            isEating = 3,
            foodSpotted = 4,
            fullStomach = 5,
            isSplit = 6,
            isPatrolling = 7,
            isHunting = 8
        }

        public VisionCritterA visionCritterA;
        public bool isWandering;
        public bool isHunting;
        
        public void CollectConditions(AntAIAgent aAgent, AntAICondition aWorldState)
        {
            aWorldState.Set(CritterA.isPatrolling, Wandering());
            aWorldState.Set(CritterA.isHunting, Hunting());
        }
        
        public bool Wandering()
        {
            if (isWandering == false)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public bool Hunting()
        {
            if (isHunting == false)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}

