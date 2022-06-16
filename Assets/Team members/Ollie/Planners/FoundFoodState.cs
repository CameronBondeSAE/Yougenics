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
        public List<Vector3> positionsList;
        public List<Vector3> distancesList;
        public Vector3[] distancesToFood;
        public override void Create(GameObject aGameObject)
        {
            base.Create(aGameObject);
            parent = aGameObject;
            parentPos = parent.transform.position;
        }
        

        public override void Enter()
        {
            base.Enter();
            positionsList = parent.GetComponent<CritterAIPlanner>().foodLocationList;
            for (int i = 0;
                 i < positionsList.Count;
                 i++)
            {
                float distance = Vector3.Distance(positionsList[i], parentPos);
                
            }
            //need to figure out the closest food distance
            
            //transform closest = null
            //smallestdistance = 999999 (big number)
            
            //foreach through the positions
            //if Vector3.distance(singlething, me) < smallestDistance
            //smallestdistance = Vector3.distance
            //outside of the loop, closest = smallestdistance
        }

        public override void Execute(float aDeltaTime, float aTimeScale)
        {
            base.Execute(aDeltaTime, aTimeScale);
            MoveToFood();
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

        void MoveToFood()
        {
            
        }
    }
}