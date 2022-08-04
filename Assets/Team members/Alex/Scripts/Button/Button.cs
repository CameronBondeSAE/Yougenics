using UnityEngine;
using System;
using Unity.Netcode;

namespace Alex
{
    [SelectionBase]
    public class Button : NetworkBehaviour, IInteractable
    {
        public bool canInteract = true;
        public event Action buttonPressedEvent;
        
        public void Interact()
        {
            if (IsServer)
                PressButtonClientRpc();
            else
                SubmitInteractRequestServerRpc();
        }

        [ServerRpc(RequireOwnership = false)]
        private void SubmitInteractRequestServerRpc()
        {
            PressButtonClientRpc();
        }

        public void Press()
        {
            
            if (canInteract)
            {
                buttonPressedEvent?.Invoke();
            }
        }

        [ClientRpc]
        public void PressButtonClientRpc()
        {
            if (canInteract)
            {
                buttonPressedEvent?.Invoke();
            }
        }
    }
    
}