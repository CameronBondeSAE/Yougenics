using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Alex
{

    public class StateBase : MonoBehaviour
    {

        // Note the ‘virtual’ keywords. 
        // This allows this function to be ‘override’ overridden in other classes to customise what happens for each state


        public virtual void Enter()
        {
            //Overide.DebugLog("Test");
            //base.Test();    
        }

        public virtual void Execute()
        {

        }

        public virtual void Exit()
        {

        }

    }

}

