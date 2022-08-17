using System.Collections;
using System.Collections.Generic;
using Anthill.AI;
using UnityEngine;

namespace Ollie
{
    public class FoundMateState : AntAIState
    {
        private GameObject parent;
        private CritterAIPlanner brain;
        
        public List<GameObject> positionsList;
        public Transform closest;
        public override void Create(GameObject aGameObject)
        {
            base.Create(aGameObject);
            parent = aGameObject;
            brain = parent.GetComponent<CritterAIPlanner>();
        }

        public override void Enter()
        {
            base.Enter();
            brain.moveSpeed = 5;
            CheckMateDistances();
            brain.path.Clear();
        }
        
        void CheckMateDistances()
        {
            closest = null;
            positionsList = brain.mateLocationList;
            
            float smallestdistance = 99999999;

            foreach (var position in positionsList)
            {
                if (Vector3.Distance(position.transform.position, parent.transform.position) < smallestdistance)
                {
                    smallestdistance = Vector3.Distance(position.transform.position,parent.transform.position);
                    closest = position.transform;
                }
            }
        }

        public override void Execute(float aDeltaTime, float aTimeScale)
        {
            brain.StateViewerChange(3);
            base.Execute(aDeltaTime, aTimeScale);
            
            if (brain.path.Count == 0)
            {
                if (brain.transform.position != closest.position)
                {
                    brain.SetTarget(closest);
                    print("target mate set");
                }
                else
                {
                    CheckMateDistances();
                }
            }
            
            if (Vector3.Distance(parent.transform.position, closest.position) < 1)
            {
                brain.moveSpeed = 0;
                print("its GIGADY time");
                brain.SetMateFound(true);
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
