using System.Collections;
using System.Collections.Generic;
using Anthill.AI;
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
        }
    }
}