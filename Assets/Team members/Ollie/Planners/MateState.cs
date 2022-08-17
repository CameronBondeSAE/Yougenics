using System.Collections;
using System.Collections.Generic;
using Anthill.AI;
using UnityEngine;

namespace Ollie
{
    public class MateState : AntAIState
    {
        private GameObject parent;
        private CritterAIPlanner brain;
        
        private GameObject target;
        private bool doneMating;
        public override void Create(GameObject aGameObject)
        {
            base.Create(aGameObject);
            doneMating = false;
            parent = aGameObject;
            brain = aGameObject.GetComponent<CritterAIPlanner>();
        }

        public override void Enter()
        {
            base.Enter();
            target = brain.target.gameObject;
            brain.turnTowards.TurnParent(target.transform.position);
            StartCoroutine(MateCoroutine());
        }

        public override void Execute(float aDeltaTime, float aTimeScale)
        {
            brain.StateViewerChange(3);
            brain.turnTowards.TurnParent(target.transform.position);
            base.Execute(aDeltaTime, aTimeScale);

            if (doneMating)
            {
                print("done mating");
                doneMating = false;
            }
        }

        public override void Exit()
        {
            base.Exit();
            brain.SetMateFound(false);
            brain.SetMateLocated(false);
        }

        public override void Destroy(GameObject aGameObject)
        {
            base.Destroy(aGameObject);
        }
        
        public IEnumerator MateCoroutine()
        {
            print("coroutine started");
            yield return new WaitForSeconds(5);
            
            
            //need to implement MateMe here!!!
            //spawn another critter basically
            brain.moveSpeed = 3;
            brain.SetIsHorny(false);
            doneMating = true;
        }
    }
}
