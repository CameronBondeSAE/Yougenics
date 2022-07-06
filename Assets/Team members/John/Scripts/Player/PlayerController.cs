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
            if (!IsOwner)
                Destroy(this);
        }

        public void OnPlayerAssigned()
        {
            playerInput.actions.FindAction("Interact").performed += aContext => playerModel.Interact();
            playerInput.actions.FindAction("Jump").performed += aContext => playerModel.Jump();

            //Player Movement
            playerInput.actions.FindAction("Movement").performed += OnMovementOnperformed;
            playerInput.actions.FindAction("Movement").canceled += OnMovementOnperformed;

            //Player Look Direction
            playerInput.actions.FindAction("MouseX").performed += aContext => playerModel.mouseX = aContext.ReadValue<float>();
            playerInput.actions.FindAction("MouseY").performed += aContext => playerCameraModel.mouseY = aContext.ReadValue<float>();

        }

        private void OnMovementOnperformed(InputAction.CallbackContext aContext)
        {
            if (aContext.phase == InputActionPhase.Performed)
            {
                playerModel.Movement(aContext.ReadValue<Vector2>());
            }
            else if (aContext.phase == InputActionPhase.Canceled)
            {
                playerModel.Movement(Vector2.zero);
            }
        }
    }
}
