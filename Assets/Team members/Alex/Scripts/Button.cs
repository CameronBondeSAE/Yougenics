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

       // public float interactDistance = 10f;
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
/*
        public void Update()
        {
            //CheckForPlayer();
        }

        public void CheckForPlayer()
        {

            RaycastHit hitInfo;
            hitInfo = new RaycastHit();

            if (Physics.Raycast(transform.position, transform.forward,
                    out hitInfo, interactDistance, 255)) 
            {

                Debug.DrawRay(transform.position, transform.forward * hitInfo.distance,
                    Color.red);

                Debug.Log("HIT");

                canInteract = true;
            }

            else
            {
                canInteract = false;
            }

        }


*/
    }
    
}