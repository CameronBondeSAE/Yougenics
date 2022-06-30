using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

namespace John
{
    public class PlayerController : NetworkBehaviour
    {
        Vector3 input;
        public float movementSpeed = 5f;
        Rigidbody rb;

        // Start is called before the first frame update
        void Awake()
        {
            rb = GetComponent<Rigidbody>();
        }

        public override void OnNetworkSpawn()
        {
            if(!IsOwner)
            {
                Destroy(this);
            }
        }

        private void Update()
        {
            input = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        }

        void FixedUpdate()
        {
            Movement();
        }

        void Movement()
        {
            rb.velocity += input.normalized * movementSpeed;
        }
    }
}
