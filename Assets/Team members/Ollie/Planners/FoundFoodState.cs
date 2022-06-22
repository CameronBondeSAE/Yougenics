using System.Collections;
using System.Collections.Generic;
using Anthill.AI;
using Luke;
using Unity.VisualScripting;
using UnityEngine;

namespace Ollie
{
    public class FoundFoodState : AntAIState
    {
        private GameObject parent;
        private Vector3 parentPos;
        private GameObject controller;
        private BoxCollider parentCollider;
        public List<Transform> positionsList;
        public List<Vector3> distancesList;
        public Vector3[] distancesToFood;
        public Transform closest;
        public override void Create(GameObject aGameObject)
        {
            base.Create(aGameObject);
            parent = aGameObject;
            controller = aGameObject.GetComponentInChildren<Controller>().gameObject;
            parentPos = parent.transform.position;
        }
        

        public override void Enter()
        {
            base.Enter();
            closest = null;
            positionsList = parent.GetComponent<CritterAIPlanner>().foodLocationList;
            for (int i = 0;
                 i < positionsList.Count;
                 i++)
            {
                float distance = Vector3.Distance(positionsList[i].position, parentPos);
                
            }
            
            float smallestdistance = 99999999;

            foreach (var position in positionsList)
            {
                if (Vector3.Distance(position.position, parent.transform.position) < smallestdistance)
                {
                    smallestdistance = Vector3.Distance(position.position,parent.transform.position);
                }
                closest = position;
            }
            Finish();
        }

        public override void Execute(float aDeltaTime, float aTimeScale)
        {
            base.Execute(aDeltaTime, aTimeScale);
            MoveToFood(closest);
            //move to closest food
            //if food exists
            //Finish(); ---- ie, move on to EatState
            //if food doesn't exist there
            //turn food located off, which should revert state back to SearchingFoodState
        }

        public override void Exit()
        {
            base.Exit();
        }

        public override void Destroy(GameObject aGameObject)
        {
            base.Destroy(aGameObject);
        }

        void MoveToFood(Transform closest)
        {
            parent.GetComponentInChildren<Controller>().MoveToTarget(closest.position);
            parentCollider = controller.GetComponent<BoxCollider>();
            parentCollider.enabled = enabled;
        }
    }
}