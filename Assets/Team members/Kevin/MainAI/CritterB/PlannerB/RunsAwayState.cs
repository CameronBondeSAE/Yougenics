using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kevin
{
    public class RunsAwayState : CritterBAIState
    {
        public float alpha;
        public bool walkPointSet;
        public Vector3 walkPoint;
        public float walkPointRange = 10f;
        public override void Enter()
        {
            base.Enter();
            /*if (critterB.canInvis)
            {
                alpha = 0f;
                critterB.canInvis = false;
                critterB.CooldownFunction();
            }
            else
            {
                alpha = 1f;
            }*/
            alpha = 0f;
            critterB.Chameleon(alpha);
            critterB.acceleration = 850f;
            critterB.myAudioClip = critterB.animalScreech;
        }

        public override void Execute(float aDeltaTime, float aTimeScale)
        {
            base.Execute(aDeltaTime, aTimeScale);
            //GenerateNextWalkPoint();
            if (!critterB.isSleeping && critterB.isInDanger)
            {
                if (critterB.predatorList.Count > 0 && critterB.canRun)
                {
                    TurnTo((-GetClosestPredator(critterB.predatorList,this.transform).transform.position));
                    //critterB.turnTowards.Turn(-GetClosestPredator(critterB.foodList, this.transform).transform.position);
                    critterB.rb.AddRelativeForce(Vector3.forward * critterB.acceleration);
                    /*critterB.transform.position = Vector3.MoveTowards(this.transform.position, 
                        (-GetClosestPredator(critterB.predatorList,this.transform).transform.position),-1 * 0.05f);*/
                }
            }
            
            //GenerateNextWalkPoint();
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
            critterB.transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation,Time.deltaTime);
        }

        public override void Exit()
        {
            base.Exit();
            critterB.myAudioClip = critterB.snarlClip;
            critterB.acceleration = 750f;
            critterB.canRun = true;
        }

        /*public void GenerateNextWalkPoint()
        {
            float randomX = Random.Range(-walkPointRange, walkPointRange);
            float randomZ = Random.Range(-walkPointRange, walkPointRange);

            walkPoint = new Vector3(transform.position.x + randomX, transform.position.y,
                transform.position.z + randomZ);

            walkPointSet = true;
        }*/
    }
}

