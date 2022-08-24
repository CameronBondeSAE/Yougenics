using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kevin
{
    public class EatState : CritterBAIState
    {
        public override void Enter()
        {
            base.Enter();
            
        }

        public override void Execute(float aDeltaTime, float aTimeScale)
        {
            base.Execute(aDeltaTime, aTimeScale);
            /*if (critterB.isSleeping) return;
            if (currentFood == null) return;
            GameObject go = Instantiate(beam, _transform);
            go.GetComponent<Beam>().target = currentFood.transform;
            Minh.Health targetHealth = currentFood.GetComponent<Minh.Health>();
            targetHealth.ChangeHealth(-critterInfo.dangerLevel);
            IEdible targetIEdible = currentFood.GetComponent<IEdible>();
            energyComp.ChangeEnergy(targetIEdible.EatMe(-critterInfo.dangerLevel));
            energyComp.ChangeEnergy(critterInfo.dangerLevel);
            justAte = true;
            StopCoroutine(EnergyDecayCooldown());
            StartCoroutine(EnergyDecayCooldown());*/
            Debug.Log("Eating!");
        }

        public override void Exit()
        {
            base.Exit();
            critterB.isEating = false;
            critterB.foundFood = false;
            critterB.isHungry = false;
            critterB.caughtFood = false;
        }
    }
}

