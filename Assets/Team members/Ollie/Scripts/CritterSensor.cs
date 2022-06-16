using System.Collections;
using System.Collections.Generic;
using Anthill.AI;
using Unity.Netcode;
using UnityEngine;

namespace Ollie
{
    public class CritterSensor : MonoBehaviour, ISense
    {
        public CritterAIPlanner control;
        public Vision vision;

        public void CollectConditions(AntAIAgent aAgent, AntAICondition aWorldState)
        {
            vision.VisionRefresh();
            aWorldState.Set(Scenario.isSafe, control.isSafe);
            aWorldState.Set(Scenario.isHungry, control.isHungry);
            aWorldState.Set(Scenario.npcNearby, control.npcNearby);
            aWorldState.Set(Scenario.preyFound, control.preyFound);
            aWorldState.Set(Scenario.foodFound, control.foodFound);
            aWorldState.Set(Scenario.predatorFound, control.predatorFound);
            aWorldState.Set(Scenario.mateFound, control.mateFound);
            aWorldState.Set(Scenario.findingPrey, control.findingPrey);
            aWorldState.Set(Scenario.findingFood, control.foodLocated);
            aWorldState.Set(Scenario.findingPredator, control.findingPredator);
            aWorldState.Set(Scenario.findingMate, control.findingMate);
            aWorldState.Set(Scenario.isHorny, control.isHorny);
            aWorldState.Set(Scenario.healthLow, control.healthLow);
            aWorldState.Set(Scenario.runningAway, control.runningAway);
            aWorldState.Set(Scenario.inDanger, control.inDanger);
            aWorldState.Set(Scenario.foodNearby, control.foodNearby);
            aWorldState.Set(Scenario.preyNearby, control.preyNearby);
            aWorldState.Set(Scenario.mateNearby, control.mateNearby);
            aWorldState.Set(Scenario.predatorNearby, control.predatorNearby);
            aWorldState.Set(Scenario.preyDead, control.preyDead);
            
        }
    }
}