using System;
using System.Collections;
using System.Collections.Generic;
using Anthill.AI;
using UnityEngine;
using UnityEngine.AI;

namespace Maya
{ 
    public class FindFoodState : AntAIState
    {
        public Vision myVision;
        //private NavMeshAgent agent;
        public static List<Food> foodIWant;

        public override void Create(GameObject aGameObject)
        {
            base.Create(aGameObject);
            myVision = aGameObject.GetComponent<Vision>();
        }

        public override void Enter()
        {
            base.Enter();
            //agent = GetComponentInParent<NavMeshAgent>();
            //agent.speed = 10;
        }

        public override void Execute(float aDeltaTime, float aTimeScale)
        {
            base.Execute(aDeltaTime, aTimeScale);
            if (myVision.foodIveSeen != null)
            {
                foreach (Food piece in myVision.foodIveSeen)
                {
                    foodIWant.Add(piece);
                }
            }
        }

        public override void Exit()
        {
            base.Exit();
            Finish();
        }
    }
}