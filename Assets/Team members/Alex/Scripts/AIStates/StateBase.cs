using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Alex
{

    public class StateBase : MonoBehaviour
    {

        public StateBase()
        {
            Debug.Log("Testing");
        }

        // Note the �virtual� keywords. 
        // This allows this function to be �override� overridden in other classes to customise what happens for each state

        public virtual void EatingTest()
        {
            Debug.Log("Eating Test Done");
        }

        /*
        public virtual void Enter()
        {
            currentState = newState;
        }
        
        public virtual void Execute()
        {
            newState == currentState;
        }

        public virtual void Exit()
        {
            currentState.enabled = false;
        }
        
        
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
        */
        
    }

}

