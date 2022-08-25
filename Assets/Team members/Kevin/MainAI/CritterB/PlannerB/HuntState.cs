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
        //public TurnTowards turnTowards;
        //public Vector3 targetPrey;
        public override void Enter()
        {
            base.Enter();
            //targetPrey = GetClosestPrey(critterB.foodList, this.transform).transform.position;
        }

        public override void Execute(float aDeltaTime, float aTimeScale)
        {
            base.Execute(aDeltaTime, aTimeScale);
            if (!critterB.isHunting && critterB.caughtFood) return;
            //RaycastHit hitInfo;
            
            if (critterB.foodList.Count != 0)
            {
                //critterB.turnTowards.Turn(GetClosestPrey(critterB.foodList, this.transform).transform.position);
                TurnTo(GetClosestPrey(critterB.foodList, this.transform).transform.position);
                critterB.transform.position = Vector3.MoveTowards(this.transform.position, 
                    GetClosestPrey(critterB.foodList, this.transform).transform.position,0.075f);

                if (critterB.transform.position == GetClosestPrey(critterB.foodList, this.transform).transform.position)
                {
                    critterB.caughtFood = true;
                }
                /*if(Physics.Raycast(transform.position,GetClosestPrey(critterB.foodList, this.transform).transform.position, out hitInfo, 2.5f,255,QueryTriggerInteraction.Ignore))
                {
                    Debug.Log("in the mouth!");
                    Debug.DrawRay(transform.position,GetClosestPrey(critterB.foodList, this.transform).transform.position,Color.cyan);
                    if (hitInfo.collider.GetComponent<IEdible>() != null)
                    {
                        critterB.foundFood = false;
                        critterB.caughtFood = true;
                        critterB.isEating = true;
                        Debug.Log("in the mouth!");
                    }
                }*/
            }

            /*RaycastHit hitInfo;
            
            
            if(Physics.Raycast(transform.position,GetClosestPrey(critterB.foodList, this.transform).transform.position, out hitInfo, 2.5f,255,QueryTriggerInteraction.Ignore))
            {
                Debug.Log("in the mouth!");
                Debug.DrawRay(transform.position,GetClosestPrey(critterB.foodList, this.transform).transform.position,Color.cyan);
                if (hitInfo.collider.GetComponent<IEdible>() != null)
                {
                    critterB.foundFood = false;
                    critterB.caughtFood = true;
                    critterB.isEating = true;
                    Debug.Log("in the mouth!");
                }
            }*/
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
    }
}

