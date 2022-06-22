using System;
using System.Collections;
using System.Collections.Generic;
using Anthill.AI;
using UnityEngine;

namespace Ollie
{
    public class SearchingFoodState : AntAIState
    {
        private GameObject parent;
        public float moveTime;
        public override void Create(GameObject aGameObject)
        {
            base.Create(aGameObject);
            parent = aGameObject;
        }

        public override void Enter()
        {
            base.Enter();
            moveTime = 5;
            Finish();
        }

        public override void Execute(float aDeltaTime, float aTimeScale)
        {
            base.Execute(aDeltaTime, aTimeScale);
            if (parent.GetComponent<CritterAIPlanner>().foodLocationList.Count == 0)
            {
                StandardMovement();
            }
            else
            {
                parent.GetComponent<CritterAIPlanner>().SetFoodLocated();
                parent.GetComponentInChildren<Controller>().StopMovement();
                Finish();
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
        
        public void StandardMovement()
        {
            if (parent.transform.position == parent.GetComponentInChildren<Controller>().targetLocation)// && !interactingTarget)
            {
                StartCoroutine(RandomLocation());
            }
        }

        public IEnumerator RandomLocation()
        {
            parent.GetComponentInChildren<Controller>().targetLocation = new Vector3((UnityEngine.Random.Range(-40,40)), 1, (UnityEngine.Random.Range(-40,40)));
            yield return new WaitForSeconds(moveTime);
            print("noLocationSet = true");
        }
    }
}
