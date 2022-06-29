using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

namespace John
{
    public class PlayerController : NetworkBehaviour
    {
        public CharacterController controller;
        public float movementSpeed = 5f;

        // Start is called before the first frame update
        void Start()
        {
            controller = gameObject.AddComponent<CharacterController>();
        }

        public override void OnNetworkSpawn()
        {
            if(!IsOwner)
            {
                Destroy(this);
            }
        }

        void FixedUpdate()
        {
            if(IsServer)
            {
                Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
                controller.Move(move * Time.deltaTime * movementSpeed);
            }
            else
            {
                SubmitMovementServerRpc();
            }

        }

        [ServerRpc]
        void SubmitMovementServerRpc()
        {
            Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            controller.Move(move * Time.deltaTime * movementSpeed);
        }
    }
}
