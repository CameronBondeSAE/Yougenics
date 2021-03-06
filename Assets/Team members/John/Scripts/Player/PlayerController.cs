using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Netcode;
using System;

namespace John
{
    public class PlayerController : NetworkBehaviour
    {
        public PlayerInput playerInput;
        public PlayerModel playerModel;

        bool inMenu = false;

        public override void OnNetworkSpawn()
        {
            if(IsLocalPlayer)
            {
                OnPlayerAssigned();
            }
        }

        #region Non-Network Setup
        public void OnPlayerAssignedNonNetworked(PlayerModel player)
        {
            playerModel = player;
            playerInput = player.playerInput;

            playerInput.actions.FindAction("Interact").performed += aContext => Interact();
            playerInput.actions.FindAction("Jump").performed += aContext => Jump();

            //Player Movement
            playerInput.actions.FindAction("Movement").performed += OnMovementOnperformed;
            playerInput.actions.FindAction("Movement").canceled += OnMovementOnperformed;

            //Player Look Direction
            playerInput.actions.FindAction("MouseX").performed += aContext => PlayerMouseX(aContext.ReadValue<float>());
            playerInput.actions.FindAction("MouseY").performed += aContext => PlayerMouseY(aContext.ReadValue<float>());
        }

        private void OnMovementOnperformed(InputAction.CallbackContext obj)
        {
            if (obj.phase == InputActionPhase.Performed)
            {
                playerModel.Movement(obj.ReadValue<Vector2>());
            }
            else if (obj.phase == InputActionPhase.Canceled)
            {
                playerModel.Movement(Vector2.zero);
            }
        }

        private void PlayerMouseY(float v)
        {
            playerModel.MouseY(v);
        }

        private void PlayerMouseX(float v)
        {
            playerModel.MouseX(v);
        }

        private void Jump()
        {
            playerModel.Jump();
        }

        private void Interact()
        {
            playerModel.Interact();
        }
        #endregion

        #region Networked Setup
        public void OnPlayerAssigned()
        {
            playerInput.actions.FindAction("Interact").performed += aContext => RequestInteractServerRpc();
            playerInput.actions.FindAction("Jump").performed += aContext => RequestJumpServerRpc();

            //Player Movement
            playerInput.actions.FindAction("Movement").performed += OnMovementOnperformedNetworked;
            playerInput.actions.FindAction("Movement").canceled += OnMovementOnperformedNetworked;

            //Player Look Direction
            playerInput.actions.FindAction("MouseX").performed += aContext => RequestPlayerMouseXServerRpc(aContext.ReadValue<float>());
            playerInput.actions.FindAction("MouseY").performed += aContext => RequestPlayerMouseYServerRpc(aContext.ReadValue<float>());

            playerInput.actions.FindAction("Menu").performed += aContext => RequestLobbyDisplay();
        }

        public void OnPlayerUnassigned()
        {
            playerInput.actions.FindAction("Interact").performed -= aContext => RequestInteractServerRpc();
            playerInput.actions.FindAction("Jump").performed -= aContext => RequestJumpServerRpc();

            //Player Movement
            playerInput.actions.FindAction("Movement").performed -= OnMovementOnperformedNetworked;
            playerInput.actions.FindAction("Movement").canceled -= OnMovementOnperformedNetworked;

            //Player Look Direction
            playerInput.actions.FindAction("MouseX").performed -= aContext => RequestPlayerMouseXServerRpc(aContext.ReadValue<float>());
            playerInput.actions.FindAction("MouseY").performed -= aContext => RequestPlayerMouseYServerRpc(aContext.ReadValue<float>());

            playerInput.actions.FindAction("Menu").performed -= aContext => RequestLobbyDisplay();
        }

        private void RequestLobbyDisplay()
        {
            if(inMenu)
            {
                inMenu = false;
            }
            else
            {
                inMenu = true;
            }

            LobbyUIManager.instance.DisplayLobby(inMenu);
        }

        [ServerRpc]
        void RequestJumpServerRpc()
        {
            playerModel?.JumpClientRpc();
        }

        [ServerRpc]
        void RequestInteractServerRpc()
        {
            playerModel?.InteractClientRpc();
        }

        [ServerRpc]
        void RequestPlayerMouseXServerRpc(float value)
        {
            if (playerModel != null)
                playerModel.MouseXClientRpc(value);
        }

        [ServerRpc]
        void RequestPlayerMouseYServerRpc(float value)
        {
            if(playerModel != null)
                playerModel.MouseYClientRpc(value);
        }

        private void OnMovementOnperformedNetworked(InputAction.CallbackContext aContext)
        {
            if (aContext.phase == InputActionPhase.Performed)
            {
                RequestMovementServerRpc(aContext.ReadValue<Vector2>());
            }
            else if (aContext.phase == InputActionPhase.Canceled)
            {
                RequestMovementServerRpc(Vector2.zero);
            }
        }

        [ServerRpc]
        void RequestMovementServerRpc(Vector2 input)
        {
            playerModel?.MovementClientRpc(input);
        }
        #endregion
    }
}
