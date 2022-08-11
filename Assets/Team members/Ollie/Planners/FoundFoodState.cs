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
        private CritterAIPlanner brain;
        
        private Vector3 parentPos;
        private Controller controller;
        private BoxCollider parentCollider;
        public List<Transform> positionsList;
        public List<Vector3> distancesList;
        public Vector3[] distancesToFood;
        public Transform closest;

        public override void Create(GameObject aGameObject)
        {
            base.Create(aGameObject);
            parent = aGameObject;
            brain = parent.GetComponent<CritterAIPlanner>();
            controller = aGameObject.GetComponentInChildren<Controller>();
            parentPos = parent.transform.position;
            parentCollider = controller.GetComponent<BoxCollider>();
        }
        

        public override void Enter()
        {
            base.Enter();
            CheckFoodDistances();
            brain.path.Clear();
        }

        void CheckFoodDistances()
        {
            closest = null;
            positionsList = parent.GetComponent<CritterAIPlanner>().foodLocationList;
            
            // for (int i = 0;
            //      i < positionsList.Count;
            //      i++)
            // {
            //     float distance = Vector3.Distance(positionsList[i].position, parentPos);
            // }
            
            float smallestdistance = 99999999;

            foreach (var position in positionsList)
            {
                if (Vector3.Distance(position.position, parent.transform.position) < smallestdistance)
                {
                    smallestdistance = Vector3.Distance(position.position,parent.transform.position);
                }
                closest = position;
            }
        }

        public override void Execute(float aDeltaTime, float aTimeScale)
        {
            base.Execute(aDeltaTime, aTimeScale);
            
            if (brain.path.Count == 0)
            {
                if (brain.transform.position != closest.position)
                {
                    brain.SetTarget(closest);
                }
                else
                {
                    parentCollider.enabled = enabled;
                    CheckFoodDistances();
                }
                
                //else
                //invoke Event to turn on Controller's collider
                //if it collides with food, controller will set bool to progress planner
                //pick new closest target
                    //will be overwritten if planner progresses
                
                
                
                
                /*else if (!foodExists)
                {
                    //this needs to check if food exists before exiting
                    //trigger could fire off event?
                    //change trigger to coroutine so it returns whatever hit?
                    //if null, disable collider and then move to next closest food?
                    
                    parentCollider.enabled = enabled;
                }
                else if (foodMissing)
                {
                    parentCollider.enabled = !enabled;
                }
                else
                {
                    parentCollider.enabled = !enabled;
                    brain.SetFoodFound(true);
                    base.Exit();
                }*/
            }
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
            
            
        }
    }
}