using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ollie
{
    public enum Scenario
    {
        isSafe = 0,
        isHungry = 1,
        npcNearby = 2,
        preyFound = 3,
        foodFound = 4,
        predatorFound = 5,
        mateFound = 6,
        findingPrey = 7,
        findingFood = 8,
        findingPredator = 9,
        findingMate = 10,
        isHorny = 11,
        healthLow = 12,
        runningAway = 13,
        inDanger = 14,
        foodNearby = 15,
        preyNearby = 16,
        mateNearby = 17,
        predatorNearby = 18,
        preyDead = 19
    }
}