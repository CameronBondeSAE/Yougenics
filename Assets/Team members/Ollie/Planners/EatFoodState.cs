using System.Collections;
using System.Collections.Generic;
using Anthill.AI;
using UnityEngine;

namespace Ollie
{
    public class EatFoodState : AntAIState
    {
        private GameObject parent;
        private Controller controller;
        private GameObject target;
        private bool doneEating;
        private CritterAIPlanner brain;
        public override void Create(GameObject aGameObject)
        {
            base.Create(aGameObject);
            doneEating = false;
            parent = aGameObject;
            brain = aGameObject.GetComponent<CritterAIPlanner>();
            controller = aGameObject.GetComponentInChildren<Controller>();
        }

        public override void Enter()
        {
            base.Enter();
            target = controller.target;
            controller.target = null;
            controller.DisableCollider();
            StartCoroutine(EatFoodCoroutine());
            //Finish();
        }

        public override void Execute(float aDeltaTime, float aTimeScale)
        {
            base.Execute(aDeltaTime, aTimeScale);
            if (doneEating)
            {
                print("done eating");
                doneEating = false;
            }
        }

        public override void Exit()
        {
            base.Exit();
            brain.SetFoodLocated(false);
            brain.SetFoodFound(false);
            print("exiting eating");
        }

        public override void Destroy(GameObject aGameObject)
        {
            base.Destroy(aGameObject);
        }

        public IEnumerator EatFoodCoroutine()
        {
            yield return new WaitForSeconds(5);
            Destroy(target);
            //testing to move into Mating phase
            parent.GetComponent<CritterAIPlanner>().SetIsHungry(false);
            //parent.GetComponent<CritterAIPlanner>().SetIsHorny(true);
            print("in coroutine");
            doneEating = true;
        }
    }
}
