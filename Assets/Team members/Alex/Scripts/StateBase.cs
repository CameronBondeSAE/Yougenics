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

        // Note the ‘virtual’ keywords. 
        // This allows this function to be ‘override’ overridden in other classes to customise what happens for each state

        public virtual void EatingTest()
        {
            Debug.Log("Eating Test Done");
        }

        /*
        public virtual void Enter()
        {
            
        }
        
        public virtual void Execute()
        {

        }

        public virtual void Exit()
        {

        }
        */
    }

}

