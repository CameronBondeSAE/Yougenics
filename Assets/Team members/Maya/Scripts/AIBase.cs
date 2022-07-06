using System;
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
        public Horny myHorny;
        public NavMeshAgent myAgent;
        
        public float sleepEnergyRegen = -3f;
        public float energyBySpeed;
        public float defaultEnergyDrain = 1;
        public override void Create(GameObject aGameObject)
        {
            base.Create(aGameObject);
            myVision = aGameObject.GetComponentInChildren<Vision>();
            myEnergy = aGameObject.GetComponentInChildren<Energy>();
            myEnergy.energyMax += 100;
            myTouch = aGameObject.GetComponentInChildren<Touch>();
            myHorny = aGameObject.GetComponent<Horny>();
            myAgent = aGameObject.GetComponent<NavMeshAgent>();
        }

        public void UpdateEnergyDrainAmount()
        {
            energyBySpeed = (myEnergy.drainAmount * myAgent.speed) / 25 ;
        }
    }
}
