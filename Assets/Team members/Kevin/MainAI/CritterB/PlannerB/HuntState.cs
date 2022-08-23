using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Kevin;
using ParrelSync.NonCore;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

namespace Kevin
{
    public class HuntState : CritterBAIState
    {
        public override void Enter()
        {
            base.Enter();
        }

        public override void Execute(float aDeltaTime, float aTimeScale)
        {
            base.Execute(aDeltaTime, aTimeScale);
            if (!critterB.isHunting && critterB.caughtFood) return;
            
            if (critterB.foodList.Count != 0)
            {
                TurnTo(GetClosestPrey(critterB.foodList,this.transform).transform.position);
                critterB.transform.position = Vector3.MoveTowards(this.transform.position, 
                    GetClosestPrey(critterB.foodList,this.transform).transform.position,0.075f);
            }

            RaycastHit hitInfo;
            
            if(transform.position == (GetClosestPrey(critterB.foodList,transform).transform.position) || Physics.Raycast(transform.position,Vector3.forward, out hitInfo, 2.5f,255,QueryTriggerInteraction.Ignore))
            {
                Debug.Log("in the mouth!");
                Exit();
            }
        }
        private void TurnTo(Vector3 target)
        {
            Quaternion targetRotation = Quaternion.LookRotation(target - transform.position);
            critterB.transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation,Time.deltaTime);
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

        public override void Exit()
        {
            base.Exit();
            critterB.foundFood = false;
            critterB.isEating = true;
        }
    }
}

