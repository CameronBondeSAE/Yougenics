using System.Collections;
using System.Collections.Generic;
using Alex;
using Anthill.Effects;
using DG.Tweening;
using UnityEditor;
using UnityEngine;
using System;
using Unity.Netcode;

namespace Alex
{
    public class Button : NetworkBehaviour, IInteractable
    {
        public bool canInteract = true;
        public event Action buttonPressedEvent;
        
        public void Interact()
        {
            if (IsServer)
                Press();
            else
                SubmitInteractRequestServerRpc();
        }

        [ServerRpc(RequireOwnership = false)]
        private void SubmitInteractRequestServerRpc()
        {
            Interact();
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