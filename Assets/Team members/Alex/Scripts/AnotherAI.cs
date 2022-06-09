using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Alex;


namespace Alex
{
    public class AnotherAI : MonoBehaviour
    {
        public float movementSpeed;
        public float lookingSpeed = 1f;
        public Transform Target;
        public Alex.Food myFoodTarget;
        Energy myEnergy;
        Rigidbody rb;

        // Start is called before the first frame update
        void Start()
        {
            /*
            StateBase stateBase = new StateBase();

            stateBase.EatingTest();

            Eating eating = new Eating();
            eating.EatingTest();

            */

            ChangeState(GetComponent<LookingForFood>());
            
        }

        
        public StateBase currentState;

        private void Update()
        {
            if (GetComponent<Energy>().energyAmount <= 10)
            {
                // Sleep
                ChangeState(GetComponent<Sleeping>());
            }
            else if (GetComponent<Energy>().energyAmount < 50)
            {
                // Look for food
                ChangeState(GetComponent<LookingForFood>());
            }
        }

        // This works for ANY STATE
        public void ChangeState(StateBase newState)
        {
            // Check if the state is the same and DON'T swap
            if (newState == currentState)
            {
                return;
            }

            // At first 'currentstate' will ALWAYS be null
            if (currentState != null)
            {
                currentState.enabled = false;
            }

            newState.enabled = true;

            // New state swap over to incoming state
            currentState = newState;
        }




    }
}
