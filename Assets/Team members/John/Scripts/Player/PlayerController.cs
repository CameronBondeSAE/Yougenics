using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Netcode;

namespace John
{
    public class PlayerController : NetworkBehaviour
    {
        public PlayerInput playerInput;
        public PlayerModel playerModel;
        public PlayerCameraModel playerCameraModel;

        public override void OnNetworkSpawn()
        {
            if(IsLocalPlayer)
            {
                OnPlayerAssigned();
            }
        }

        public void OnPlayerAssigned()
        {
            playerInput.actions.FindAction("Interact").performed += aContext => RequestInteractServerRpc();
            playerInput.actions.FindAction("Jump").performed += aContext => RequestJumpServerRpc();

            //Player Movement
            playerInput.actions.FindAction("Movement").performed += OnMovementOnperformed;
            playerInput.actions.FindAction("Movement").canceled += OnMovementOnperformed;

            //Player Look Direction
            playerInput.actions.FindAction("MouseX").performed += aContext => RequestPlayerMouseXServerRpc(aContext.ReadValue<float>());
            playerInput.actions.FindAction("MouseY").performed += aContext => RequestPlayerMouseYServerRpc(aContext.ReadValue<float>());
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
            playerModel?.MouseXClientRpc(value);
        }

        [ServerRpc]
        void RequestPlayerMouseYServerRpc(float value)
        {
            playerModel?.MouseYClientRpc(value);
        }

        private void OnMovementOnperformed(InputAction.CallbackContext aContext)
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
    }
}
