using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cam
{
    public class Button : MonoBehaviour
    {
        public event Action PressedEvent;
        public event Action FinishedEvent;
        
        [Button]
        public void Press()
        {
            PressedEvent?.Invoke();   
        }

        [Button]
        public void Finish()
        {
            FinishedEvent?.Invoke();
        }
    }

}