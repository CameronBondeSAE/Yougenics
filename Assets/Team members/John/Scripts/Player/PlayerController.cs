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

        #region Networked Setup
        public void OnPlayerAssigned()
        {
            // Lock cursor to window
            Cursor.lockState = CursorLockMode.Locked;
            
            playerInput.actions.Enable();

            playerInput.actions.FindAction("Interact").performed += aContext => RequestInteractServerRpc();
            playerInput.actions.FindAction("Jump").performed += aContext => RequestJumpServerRpc();

            //Player Movement
            playerInput.actions.FindAction("Movement").performed += OnMovementOnperformedNetworked;
            playerInput.actions.FindAction("Movement").canceled += OnMovementOnperformedNetworked;

            playerInput.actions.FindAction("Sprint").performed += OnSprintOnperformedNetworked;
            playerInput.actions.FindAction("Sprint").canceled += OnSprintOnperformedNetworked;

            //Player Look Direction
            playerInput.actions.FindAction("MouseX").performed += aContext => RequestPlayerMouseXServerRpc(aContext.ReadValue<float>());
            playerInput.actions.FindAction("MouseY").performed += aContext => RequestPlayerMouseYServerRpc(aContext.ReadValue<float>());

            playerInput.actions.FindAction("OpenMenu").performed += aContext => ShowLobby();
            playerInput.actions.FindAction("CloseMenu").performed += aContext => HideLobby();
        }
        public void OnPlayerAssignedUsingClientSidePredictition()
        {
            playerInput.actions.Enable();

            playerInput.actions.FindAction("Interact").performed += aContext => Interact();
            playerInput.actions.FindAction("Jump").performed += aContext => Jump();

            //Player Movement
            playerInput.actions.FindAction("Movement").performed += OnMovementOnperformed;
            playerInput.actions.FindAction("Movement").canceled += OnMovementOnperformed;

            playerInput.actions.FindAction("Sprint").performed += OnSprintOnperformedNetworked;
            playerInput.actions.FindAction("Sprint").canceled += OnSprintOnperformedNetworked;

            //Player Look Direction
            playerInput.actions.FindAction("MouseX").performed += aContext => PlayerMouseX(aContext.ReadValue<float>());
            playerInput.actions.FindAction("MouseY").performed += aContext => PlayerMouseY(aContext.ReadValue<float>());

            playerInput.actions.FindAction("OpenMenu").performed += aContext => ShowLobby();
            playerInput.actions.FindAction("CloseMenu").performed += aContext => HideLobby();

            //Can subscribe to the player dying and disable controls using OnPlayerUnassigned here?
            //playerModel.onDeathEvent += DisableControls;
        }

        public void OnPlayerUnassigned()
        {
            playerInput.actions.FindAction("Interact").performed -= aContext => RequestInteractServerRpc();
            playerInput.actions.FindAction("Jump").performed -= aContext => RequestJumpServerRpc();

            //Player Movement
            playerInput.actions.FindAction("Movement").performed -= OnMovementOnperformedNetworked;
            playerInput.actions.FindAction("Movement").canceled -= OnMovementOnperformedNetworked;

            playerInput.actions.FindAction("Sprint").performed -= OnSprintOnperformedNetworked;
            playerInput.actions.FindAction("Sprint").canceled -= OnSprintOnperformedNetworked;

            //Player Look Direction
            playerInput.actions.FindAction("MouseX").performed -= aContext => RequestPlayerMouseXServerRpc(aContext.ReadValue<float>());
            playerInput.actions.FindAction("MouseY").performed -= aContext => RequestPlayerMouseYServerRpc(aContext.ReadValue<float>());

            playerInput.actions.FindAction("OpenMenu").performed -= aContext => ShowLobby();
            playerInput.actions.FindAction("CloseMenu").performed -= aContext => HideLobby();

            playerInput.actions.Disable();
        }
        public void OnPlayerUnassignedUsingClientSidePredictition()
        {
            playerInput.actions.FindAction("Interact").performed -= aContext => Interact();
            playerInput.actions.FindAction("Jump").performed -= aContext => Jump();

            //Player Movement
            playerInput.actions.FindAction("Movement").performed -= OnMovementOnperformed;
            playerInput.actions.FindAction("Movement").canceled -= OnMovementOnperformed;

            playerInput.actions.FindAction("Sprint").performed -= OnSprintOnperformedNetworked;
            playerInput.actions.FindAction("Sprint").canceled -= OnSprintOnperformedNetworked;

            //Player Look Direction
            playerInput.actions.FindAction("MouseX").performed -= aContext => PlayerMouseX(aContext.ReadValue<float>());
            playerInput.actions.FindAction("MouseY").performed -= aContext => PlayerMouseY(aContext.ReadValue<float>());

            playerInput.actions.FindAction("OpenMenu").performed -= aContext => ShowLobby();
            playerInput.actions.FindAction("CloseMenu").performed -= aContext => HideLobby();

            playerInput.actions.Disable();
        }

        //BUG: Both ShowLobby & HideLobby get called when esc is first pressed
        public void ShowLobby()
        {
            playerInput.SwitchCurrentActionMap("InMenu");

            LobbyUIManager.instance.DisplayLobby(true);
        }

        public void HideLobby()
        {
            playerInput.SwitchCurrentActionMap("InGame");

            LobbyUIManager.instance.DisplayLobby(false);
        }

        [ServerRpc]
        void RequestJumpServerRpc()
        {
            playerModel?.Jump();
            playerModel?.JumpClientRpc();
        }

        [ServerRpc]
        void RequestInteractServerRpc()
        {
            playerModel?.Interact();
            playerModel?.InteractClientRpc();
        }

        [ServerRpc]
        void RequestPlayerMouseXServerRpc(float value)
        {
            if (playerModel != null)
            {
                playerModel.MouseX(value);

                playerModel.MouseXClientRpc(value);
            }
        }

        [ServerRpc]
        void RequestPlayerMouseYServerRpc(float value)
        {
            if(playerModel != null)
            {
                playerModel.MouseY(value);

                playerModel.MouseYClientRpc(value);
            }
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
            playerModel?.Movement(input);
            
            playerModel?.MovementClientRpc(input);
        }

        private void OnSprintOnperformedNetworked(InputAction.CallbackContext aContext)
        {
            if (aContext.phase == InputActionPhase.Performed)
            {
                playerModel.Sprint(true);
                RequestSprintServerRpc(true);
            }
            else if (aContext.phase == InputActionPhase.Canceled)
            {
                playerModel.Sprint(false);
                RequestSprintServerRpc(false);
            }
        }

        [ServerRpc]
        void RequestSprintServerRpc(bool isSprinting)
        {
            playerModel?.Sprint(isSprinting);

            playerModel?.SprintClientRpc(isSprinting);
        }
        #endregion

        #region Client Side Prediction
        private void OnMovementOnperformed(InputAction.CallbackContext obj)
        {
            if (obj.phase == InputActionPhase.Performed)
            {
                playerModel.Movement(obj.ReadValue<Vector2>());
                RequestMovementServerRpc(obj.ReadValue<Vector2>());
            }
            else if (obj.phase == InputActionPhase.Canceled)
            {
                playerModel.Movement(Vector2.zero);
                RequestMovementServerRpc(Vector2.zero);
            }
        }

        private void PlayerMouseY(float v)
        {
            if(playerModel != null)
            {
                playerModel.MouseY(v);

                RequestPlayerMouseYServerRpc(v);
            }
        }

        private void PlayerMouseX(float v)
        {
            if (playerModel != null)
            {
                playerModel.MouseX(v);

                RequestPlayerMouseXServerRpc(v);
            }
        }

        private void Jump()
        {
            if (playerModel != null)
            {
                if(!IsServer)
                    playerModel.Jump();

                RequestJumpServerRpc();
            }
        }

        private void Interact()
        {
            if (playerModel != null)
            {
                //playerModel.Interact();

                RequestInteractServerRpc();
            }
        }
        #endregion
    }
}
