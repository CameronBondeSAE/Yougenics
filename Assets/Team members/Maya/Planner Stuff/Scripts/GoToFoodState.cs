using System.Collections;
using System.Collections.Generic;
using Anthill.AI;
using UnityEngine;
using UnityEngine.AI;

namespace Maya
{ 
    public class GoToFoodState : AIBase
    {
        public FindFoodState findFoodState;
        public Vector3 newPos;
        
        public override void Create(GameObject aGameObject)
        {
            base.Create(aGameObject);
            findFoodState = GetComponentInChildren<FindFoodState>();
        }
        public override void Enter()
        {
            base.Enter();
            myAgent.speed = 10;
        }

        public override void Execute(float aDeltaTime, float aTimeScale)
        {
            base.Execute(aDeltaTime, aTimeScale);
            if (findFoodState.foodIWant.Count <= 0)
            {
                newPos = findFoodState.foodIWant[0].transform.position;
                myAgent.SetDestination(newPos);
            }
            else
            {
                
            }
        }

        public override void Exit()
        {
            base.Exit();
            Finish();
        }
    }
}