using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Alex
{
    public class LookingForFood : StateBase
    {
        /*
        public void Update()
        {
            LookForFood();
            MoveToFood();
        }

        // Start is called before the first frame update
        public void LookForFood()
        {
            myFoodTarget = FindObjectOfType<Alex.Food>();
            if (myFoodTarget != null)
            {
                Target = myFoodTarget.transform;
                transform.LookAt(Target);
            }

        }

        public void MoveToFood()
        {
            if (myEnergy.energyAmount <= 20 && Target != null)
            {
                //print("Low energy find food");
                rb.AddRelativeForce(0, 0, movementSpeed);
            }
            else
            {
                rb.velocity = Vector3.zero;
            }
            
        }
         */
    }
}
