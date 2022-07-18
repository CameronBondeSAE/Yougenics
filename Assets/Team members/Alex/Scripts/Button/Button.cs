using System.Collections;
using System.Collections.Generic;
using Alex;
using Anthill.Effects;
using DG.Tweening;
using UnityEditor;
using UnityEngine;
using System;

namespace Alex
{
    public class Button : MonoBehaviour, IInteractable
    {
        public bool canInteract = true;
        public event Action buttonPressedEvent;
        
        public void Interact()
        {
            Press();
        }
        
        public void Press()
        {
            
            if (canInteract)
            {
                buttonPressedEvent?.Invoke();
            }
        }
    }
    
}