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
    [SelectionBase]
    public class ShopButton : NetworkBehaviour, IInteractable
    {
        public bool canInteract = true;
        public event Action<GameObject, Transform> buttonPressedEvent;
        public GameObject myShopitem;
        public Transform mySpawnPoint;
        
        public void Interact()
        {
            //if (IsServer)
                Press();
            //else
                //SubmitInteractRequestServerRpc();
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
                buttonPressedEvent?.Invoke(myShopitem, mySpawnPoint);
            }
        }
    }
    
}