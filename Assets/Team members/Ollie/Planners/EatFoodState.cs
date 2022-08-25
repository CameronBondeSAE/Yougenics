using System.Collections;
using System.Collections.Generic;
using Anthill.AI;
using UnityEngine;

namespace Ollie
{
    public class EatFoodState : AntAIState
    {
        private GameObject parent;
        private CritterAIPlanner brain;
        
        private Controller controller;
        private GameObject target;
        private bool doneEating;
        
        public override void Create(GameObject aGameObject)
        {
            base.Create(aGameObject);
            doneEating = false;
            parent = aGameObject;
            brain = aGameObject.GetComponent<CritterAIPlanner>();
            //controller = aGameObject.GetComponentInChildren<Controller>();
        }

        public override void Enter()
        {
            base.Enter();
            if (brain.target.gameObject != null)
            {
                target = brain.target.gameObject;
                brain.turnTowards.TurnParent(target.transform.position);
                StartCoroutine(EatFoodCoroutine());
            }
            
            
            //controller.target = null;
            //controller.DisableCollider();
            
            //Finish();
        }

        public override void Execute(float aDeltaTime, float aTimeScale)
        {
            
            
            if(target!=null) brain.turnTowards.TurnParent(target.transform.position);
            base.Execute(aDeltaTime, aTimeScale);
            
            if (doneEating)
            {
                //print("done eating");
                doneEating = false;
            }
        }

        public override void Exit()
        {
            base.Exit();
            brain.SetFoodLocated(false);
            brain.SetFoodFound(false);
            //print("exiting eating");
        }

        public override void Destroy(GameObject aGameObject)
        {
            base.Destroy(aGameObject);
        }

        public IEnumerator EatFoodCoroutine()
        {
            //print("coroutine started");
            IEdible iEdible = target.GetComponent<IEdible>();
            
            while (iEdible.GetEnergyAmount() > 0)
            {
                brain.StateViewerChange(this);
                iEdible.EatMe(brain.chompAmount);
                brain.energyComponent.ChangeEnergy(brain.chompAmount);
                yield return new WaitForSeconds(1);
            }

            if (iEdible.GetEnergyAmount() <= 0 || target == null)
            {
                if (brain.GetEnergyAmount() < brain.energyComponent.energyMax / 2)
                {
                    //find new food
                    brain.SetFoodLocated(false);
                    brain.SetFoodFound(false);
                }
                else
                {
                    brain.SetIsHungry(false);
                    brain.SetHealthLow(false);
                    doneEating = true;
                }
            }
        }
    }
}
