using System.Collections;
using System.Collections.Generic;
using Anthill.AI;
using UnityEngine;

namespace Kevin
{
    
    public class RunAwayState : CritterAAIState
    {
        public bool walkPointSet;
        public Vector3 walkPoint;
        public float walkPointRange = 10f;
        
        public override void Enter()
        {
            base.Enter();
            //Change emotion UI
            //critterA.alphaFloat = Mathf.Lerp(1f, 0f, 0.1f);
            critterA.currentState = 2;
            critterA.alphaFloat = 0f;
            critterA.Chameleon();
        }

        public override void Execute(float aDeltaTime, float aTimeScale)
        {
            GenerateNextWalkPoint();
            if (!critterA.isSleeping && !critterA.isSafe)
            {
                if (critterA.predatorList.Count > 0)
                {
                    GenerateNextWalkPoint();
                    TurnTo((-GetClosestPredator(critterA.predatorList,this.transform).transform.position));
                    critterA.transform.position = Vector3.MoveTowards(this.transform.position, 
                        (-GetClosestPredator(critterA.predatorList,this.transform).transform.position),-1 * 0.05f);
                }
            }
            //critterA.RunAway();
            
        }
        Transform GetClosestPredator(List<Transform> predators, Transform thisCritter)
        {
            Transform closestTarget = null;
            float closestDistanceSquared = Mathf.Infinity;
            Vector3 currentPosition = thisCritter.position;
            foreach (Transform potentialTarget in predators)
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
        
        public void TurnTo(Vector3 target)
        {
            Quaternion targetRotation = Quaternion.LookRotation(target - transform.position);
            critterA.transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation,Time.deltaTime);
        }
            
        public void GenerateNextWalkPoint()
        {
            float randomX = Random.Range(-walkPointRange, walkPointRange);
            float randomZ = Random.Range(-walkPointRange, walkPointRange);

            walkPoint = new Vector3(transform.position.x + randomX, transform.position.y,
                transform.position.z + randomZ);

            walkPointSet = true;
        }

        public override void Exit()
        {
            base.Exit();
            critterA.alphaFloat = 1f;

        }

        /*private void TurnTo(Vector3 target)
        {
            Quaternion targetRotation = Quaternion.LookRotation(target - transform.position);
            critterA.transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation,Time.deltaTime);
        }
            
        public void GenerateNextWalkPoint()
        {
            float randomX = Random.Range(-walkPointRange, walkPointRange);
            float randomZ = Random.Range(-walkPointRange, walkPointRange);

            walkPoint = new Vector3(transform.position.x + randomX, transform.position.y,
                transform.position.z + randomZ);

            walkPointSet = true;
        }*/
    }

}
