using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Alex;


namespace Alex
{
    public class AlexAI : MonoBehaviour
    {
        Energy myEnergy;
        Rigidbody rb;
        private StateBase wandering;
        private StateBase sleeping;
        private StateBase lookingForFood;
        
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
            //If energy is greater than or equal to 100 AI will change to wondering state 
            if (GetComponent<Energy>().EnergyAmount.Value >= 100)
            {
                GetComponent<StateManager>().ChangeState(wandering);
            }

            //AI will look for food if they're energy value is between 20 and 80 and they are not in their sleeping state
            else if (myEnergy.EnergyAmount.Value < 80 && 
                     myEnergy.EnergyAmount.Value > 20 && 
                     GetComponent<StateManager>().currentState != sleeping)
            {
                // Look for food
                GetComponent<StateManager>().ChangeState(lookingForFood);
            }
            
            //If energy is at 20 or less the AI goes to sleep
            else if (myEnergy.EnergyAmount.Value <= 20)
            {
                // Sleep
                GetComponent<StateManager>().ChangeState(sleeping);
            }
        }
    }
}
