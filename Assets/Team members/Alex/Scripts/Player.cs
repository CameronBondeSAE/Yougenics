using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Alex
{
    public class Player : MonoBehaviour
    {
        public CharacterController controller;
        public float movementSpeed = 5f;
        private Vector3 playerMovement;

        public bool vehicleActive;
        public Vehicle vehicle;

        // Start is called before the first frame update
        void Start()
        {
            //Adding controller and movement to player when game starts
            //GetComponent<Vehicle>().vehicleActive = false;
            controller = gameObject.AddComponent<CharacterController>();
            Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            controller.Move(move * Time.deltaTime * movementSpeed);
        }

        // Update is called once per frame
        void Update()
        {
            //If player is not in a vehicle they have regular movements 
            //if (GetComponent<Vehicle>().vehicleActive == false)
            {
                Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
                controller.Move(move * Time.deltaTime * movementSpeed);
            }
            //If player is in vehicle movements are disabled 
            if (vehicle.vehicleActive == true)
            {
                controller.enabled = false;
            }
        }
    }
}
