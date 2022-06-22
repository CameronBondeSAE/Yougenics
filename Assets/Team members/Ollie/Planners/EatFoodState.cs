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
        public override void Create(GameObject aGameObject)
        {
            base.Create(aGameObject);
            doneEating = false;
            parent = aGameObject;
            controller = aGameObject.GetComponentInChildren<Controller>();
        }

        public override void Enter()
        {
            base.Enter();
            StartCoroutine(EatFoodCoroutine());
            //Finish();
        }

        public override void Execute(float aDeltaTime, float aTimeScale)
        {
            base.Execute(aDeltaTime, aTimeScale);
            if (doneEating)
            {
                print("done eating");
            }
        }

        public override void Exit()
        {
            base.Exit();
            print("exiting eating");
        }

        public override void Destroy(GameObject aGameObject)
        {
            base.Destroy(aGameObject);
        }

        public IEnumerator EatFoodCoroutine()
        {
            yield return new WaitForSeconds(5);
            Destroy(controller.target);
            //testing to move into Mating phase
            parent.GetComponent<CritterAIPlanner>().SetIsHungry(false);
            parent.GetComponent<CritterAIPlanner>().SetIsHorny(true);
            print("in coroutine");
            doneEating = true;
        }
    }
}
