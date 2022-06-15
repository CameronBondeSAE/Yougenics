using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Alex
{
    public class PlayerMovement : MonoBehaviour
    {
        CharacterController controller;
        public float movementSpeed = 5f;
        private Vector3 playerMovement;

        public bool vehicleActive;

        // Start is called before the first frame update
        void Start()
        {
            vehicleActive = false;
            controller = gameObject.AddComponent<CharacterController>();
        }

        // Update is called once per frame
        void Update()
        {
            if (vehicleActive == false)
            {
                Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
                controller.Move(move * Time.deltaTime * movementSpeed);
            }
            else if (vehicleActive == true)
            {
                controller.enabled = false;
            }
        }
    }
}

