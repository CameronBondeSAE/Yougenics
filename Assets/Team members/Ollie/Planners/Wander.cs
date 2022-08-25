using System.Collections;
using System.Collections.Generic;
using Anthill.AI;
using Tanks;
using UnityEngine;

namespace Ollie
{
    public class Wander : AntAIState
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
            brain.moveSpeed = 3;
            brain.target = null;
            base.Enter();
        }

        public override void Execute(float aDeltaTime, float aTimeScale)
        {
            //brain.StateViewerChange(5);
            brain.StateViewerChange(this);
            if (brain.path.Count == 0)
            {
                //brain.RandomTarget();
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