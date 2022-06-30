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
            ChangeState(wandering);
            myEnergy = GetComponent<Energy>();
        }
              
        private void Update()
        {
            if (GetComponent<Energy>().energyAmount >= 100)
            {
                ChangeState(wandering);
            }

            
            else if (myEnergy.energyAmount < 80 && 
                     myEnergy.energyAmount > 20 && 
                     currentState != sleeping)
            {
                // Look for food
                ChangeState(lookingForFood);
                
            }
            
            else if (myEnergy.energyAmount <= 20)
            {
                // Sleep
                ChangeState(sleeping);
            }
        }

        // This works for ANY STATE
        public void ChangeState(StateBase newState)
        {
            // Check if the state is the same and DON'T swap
            if (newState == currentState) return;

            // At first 'currentstate' will ALWAYS be null
            if (currentState != null) currentState.enabled = false;
            
            newState.enabled = true;

            // New state swap over to incoming state
            currentState = newState;
        }
    }
}
