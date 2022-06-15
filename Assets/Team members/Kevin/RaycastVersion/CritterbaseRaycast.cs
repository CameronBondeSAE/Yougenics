using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Kevin
{
    public class CritterbaseRaycast : MonoBehaviour, Misc.ICritter, IEntity, IEdible
    {
        public float health;

        public float energy;

        public List<Transform> surroundingEntities;

        public Misc.Gender gender;
        public Misc.FoodChain foodChain;

        //blackboard bools
        public bool isPatrolling;
        public bool isHunting;
        public bool isEating;
        public bool isFull;
        public bool isSleeping;
        public bool isMating;
        
        //blackboard variables
        public Transform closestTarget;
        public void Awake()
        {
            gender = (Misc.Gender) Random.Range(0,1);
            foodChain = (Misc.FoodChain) Random.Range(0, 2);
        }

        public void InVision(Collider other)
        {
            Debug.Log("I see you!");
            IEntity entity = GetComponent<IEntity>();
            if (entity != null)
            {
                surroundingEntities.Add(other.transform);
                isHunting = true;
            }
        }

        public void OutVision(Collider other)
        { 
            surroundingEntities.Remove(other.transform);
        }
        
        private void TurnTo(Vector3 target)
        {
            transform.LookAt(new Vector3 (target.x, transform.position.y, target.z));
            isHunting = false;
        }
        
        #region Blackboard
        
        public void Patrol()
        {
            throw new NotImplementedException();
        }

        public void Hunting()
        {
            if (isSleeping == false)
            {
                TurnTo(GetClosestTargetEntity(surroundingEntities, this.transform).transform.position);
                transform.position = Vector3.MoveTowards(this.transform.position, GetClosestTargetEntity(surroundingEntities, this.transform).transform.position,0.01f);
            }
        }

        public void Eating()
        {
            throw new NotImplementedException();
        }

        public void Sleeping()
        {
            throw new NotImplementedException();
        }

        public void Full()
        {
            throw new NotImplementedException();
        }

        public void Mating()
        {
            throw new NotImplementedException();
        }

        Transform GetClosestTargetEntity(List<Transform> entities, Transform thisNpc)
        {
            Transform closestTarget = null;
            float closestDistanceSquared = Mathf.Infinity;
            Vector3 currentPosition = thisNpc.position;
            foreach (Transform potentialTarget in entities)
            {
                Vector3 direactionToTarget = potentialTarget.position - currentPosition;
                float distanceSquareToTarget = direactionToTarget.sqrMagnitude;
                if (distanceSquareToTarget < closestDistanceSquared)
                {
                    closestDistanceSquared = distanceSquareToTarget;
                    closestTarget = potentialTarget; 
                }
            }
            //Debug.Log(closestTarget);
            return closestTarget;
        }
        #endregion
    }
}

