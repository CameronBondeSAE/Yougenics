using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Alex;


namespace Alex
{
    public class AnotherAI : MonoBehaviour
    {
        //public float movementSpeed;
        //public float lookingSpeed = 1f;
        //public Transform Target;
        //public Alex.Food myFoodTarget;
        Energy myEnergy;
        Rigidbody rb;
        public StateBase currentState;
        private StateBase wandering;
        private StateBase sleeping;
        private StateBase lookingForFood;

        // Start is called before the first frame update
        public void Awake()
        {
            wandering = GetComponent<Wandering>();
            sleeping = GetComponent<Sleeping>();
            lookingForFood = GetComponent<LookingForFood>();
            GetComponent<StateManager>().ChangeState(wandering);
            myEnergy = GetComponent<Energy>();
        }
              
        private void Update()
        {
            if (GetComponent<Energy>().energyAmount >= 100)
            {
                GetComponent<StateManager>().ChangeState(wandering);
            }

            else if (myEnergy.energyAmount < 80 && 
                     myEnergy.energyAmount > 20 && 
                     currentState != sleeping)
            {
                // Look for food
                GetComponent<StateManager>().ChangeState(lookingForFood);
            }
            
            else if (myEnergy.energyAmount <= 20)
            {
                // Sleep
                GetComponent<StateManager>().ChangeState(sleeping);
            }
        }

        // This works for ANY STATE
    }
}
