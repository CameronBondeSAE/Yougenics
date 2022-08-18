using System.Collections;
using System.Collections.Generic;
using Anthill.AI;
using UnityEngine;

namespace Kevin
{
    public class MoveToFoodState : CritterAAIState
    {
        public bool walkPointSet;
        public Vector3 walkPoint;
        public float walkPointRange = 10f;
        
        public override void Enter()
        {
            base.Enter();
            critterA.currentState = 1;
            critterA.FireEmotionsUpdate();
        }

        public override void Execute(float aDeltaTime, float aTimeScale)
        {
            base.Execute(aDeltaTime, aTimeScale);
            if (!critterA.canHunt && critterA.caughtFood) return;
            
            if (critterA.foodList.Count != 0)
            {
                TurnTo(GetClosestPrey(critterA.foodList,this.transform).transform.position);
                critterA.transform.position = Vector3.MoveTowards(this.transform.position, 
                    GetClosestPrey(critterA.foodList,this.transform).transform.position,0.075f);
            }
        }
        private void TurnTo(Vector3 target)
        {
            Quaternion targetRotation = Quaternion.LookRotation(target - transform.position);
            critterA.transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation,Time.deltaTime);
        }
        
        Transform GetClosestPrey(List<Transform> prey, Transform thisCritter)
        {
            Transform closestTarget = null;
            float closestDistanceSquared = Mathf.Infinity;
            Vector3 currentPosition = thisCritter.position;
            foreach (Transform potentialTarget in prey)
            {
                Vector3 directionToTarget = potentialTarget.position - currentPosition;
                float distanceSquareToTarget = directionToTarget.sqrMagnitude;
                if (distanceSquareToTarget < closestDistanceSquared)
                {
                    closestDistanceSquared = distanceSquareToTarget;
                    closestTarget = potentialTarget; 
                }
            }
            return closestTarget;
        }
    }

}
