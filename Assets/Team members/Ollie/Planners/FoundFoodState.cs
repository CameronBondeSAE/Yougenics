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
        public List<Transform> positionsList;
        public Transform closest;
        
        //private Vector3 parentPos;
        //private Controller controller;
        //private BoxCollider parentCollider;
        //public List<Vector3> distancesList;
        //public Vector3[] distancesToFood;
        

        public override void Create(GameObject aGameObject)
        {
            base.Create(aGameObject);
            parent = aGameObject;
            brain = parent.GetComponent<CritterAIPlanner>();
            //controller = aGameObject.GetComponentInChildren<Controller>();
            //parentPos = parent.transform.position;
            //parentCollider = controller.GetComponent<BoxCollider>();
        }
        

        public override void Enter()
        {
            base.Enter();
            CheckFoodDistances();
            brain.path.Clear();
            brain.moveSpeed = 7;
            
        }

        void CheckFoodDistances()
        {
            closest = null;
            positionsList = brain.foodLocationList;

            float smallestdistance = 99999999;

            if (positionsList.Count != 0)
            {
                foreach (var position in positionsList)
                {
                    if (position != null)
                    {
                        if (Vector3.Distance(position.position, parent.transform.position) < smallestdistance)
                        {
                            smallestdistance = Vector3.Distance(position.position,parent.transform.position);
                            closest = position;
                        }
                    }
                }
            }
        }

        public override void Execute(float aDeltaTime, float aTimeScale)
        {
            brain.StateViewerChange(1);
            base.Execute(aDeltaTime, aTimeScale);
            if (positionsList.Count == 0)
            {
                brain.SetFoodLocated(false);
            }

            for (int i = brain.foodLocationList.Count-1; i >= 0; i--)
            {
                if (brain.foodLocationList[i] != null)
                {
                }
                else brain.foodLocationList.RemoveAt(i);

                positionsList = brain.foodLocationList;
            }

            if (closest != null)
            {
                if (brain.path.Count == 0)
                {
                    if (brain.transform.position != closest.position)
                    {
                        brain.SetTarget(closest);
                        //print("target food set");
                        
                    }
                    else
                    {
                        //parentCollider.enabled = enabled;
                        CheckFoodDistances();
                    }
                }

                if (Vector3.Distance(parent.transform.position, closest.position) < 1)
                {
                    brain.moveSpeed = 0;
                    //print("its NOMming time");
                    brain.SetFoodFound(true);
                }
            }
            else
            {
                brain.foodLocationList.Remove(closest);
                positionsList = brain.foodLocationList;
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
        
    }
}