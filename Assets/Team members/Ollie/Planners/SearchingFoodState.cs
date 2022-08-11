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
        private CritterAIPlanner brain;
        public float moveTime;
        public override void Create(GameObject aGameObject)
        {
            base.Create(aGameObject);
            parent = aGameObject;
            brain = parent.GetComponent<CritterAIPlanner>();
        }

        public override void Enter()
        {
            base.Enter();
        }

        public override void Execute(float aDeltaTime, float aTimeScale)
        {
            base.Execute(aDeltaTime, aTimeScale);
            if (brain.foodLocationList.Count == 0)
            {
                if (brain.path.Count == 0)
                {
                    //brain.RandomTarget();
                }
            }
            else
            {
                parent.GetComponent<CritterAIPlanner>().SetFoodLocated(true);
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
        
    }
}
