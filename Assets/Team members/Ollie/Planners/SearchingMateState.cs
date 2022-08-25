using System.Collections;
using System.Collections.Generic;
using Anthill.AI;
using UnityEngine;

namespace Ollie
{
    public class SearchingMateState : AntAIState
    {
        private GameObject parent;
        private CritterAIPlanner brain;
        
        public override void Create(GameObject aGameObject)
        {
            parent = aGameObject;
            brain = parent.GetComponent<CritterAIPlanner>();
        }

        public override void Enter()
        {
            base.Enter();
        }

        public override void Execute(float aDeltaTime, float aTimeScale)
        {
            if (brain.sleeping == false)
            {
                //brain.StateViewerChange(3);
                brain.StateViewerChange(this);
            }
            base.Execute(aDeltaTime, aTimeScale);
            if (brain.mateLocationList.Count == 0)
            {
                
            }
            else
            {
                brain.SetMateLocated(true);
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