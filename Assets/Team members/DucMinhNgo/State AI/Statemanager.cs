using System.Collections;
using System.Collections.Generic;
using Minh;
using UnityEngine;

namespace Minh
{
    public class Statemanager : MonoBehaviour
    {
        public Basestate currentState;
        // Start is called before the first frame update

        public void ChangeState(Basestate newState)
        {
            if (newState == currentState)
            {
                return;
            }

            if (currentState != null)
            {
                currentState.enabled = false;
            }

            currentState = newState;
            newState.enabled = true;
        }
    }
}

