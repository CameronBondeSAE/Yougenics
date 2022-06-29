using System.Collections;
using System.Collections.Generic;
using Anthill.AI;
using UnityEngine;
using UnityEngine.AI;

namespace Maya
{
    
    public class AIBase : AntAIState
    {
        public Energy myEnergy;
        public Vision myVision;
        public Touch myTouch;
        public NavMeshAgent myAgent;
        public override void Create(GameObject aGameObject)
        {
            base.Create(aGameObject);
            myVision = aGameObject.GetComponentInChildren<Vision>();
            myEnergy = aGameObject.GetComponentInChildren<Energy>();
            myTouch = aGameObject.GetComponentInChildren<Touch>();
            myAgent = aGameObject.GetComponent<NavMeshAgent>();
        }
    }
}
