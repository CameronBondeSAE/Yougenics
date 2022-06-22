using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Alex
{
    public class Vehicle : MonoBehaviour
    {
        
        public Transform player;
        public bool vehicleActive;
        bool isInTransition;
        public Transform enterPoint;
        public Transform exitPoint;
        public Vector3 sittingOffSet;
        public float transitionSpeed = .2f;
        CharacterController vehicleController;
        public float movementSpeed = 5f;
        
        
        void Start()
        {
            vehicleActive = false;
            vehicleController = gameObject.AddComponent<CharacterController>();
        }
        
        private void Update()
        {
            if (vehicleActive && isInTransition)
            {
                Exit();
            }
            else if (!vehicleActive && isInTransition)
            {
                Enter();
            }

            if (Input.GetKeyDown(KeyCode.R))
            {
                isInTransition = true;
            }
        }

        void Enter()
        {
            player.GetComponent<BoxCollider>().enabled = false;
            player.GetComponent<Rigidbody>().isKinematic = true;
            player.GetComponent<Player>().controller.enabled = false;
            vehicleController.enabled = true;

            player.position = Vector3.Lerp(player.position, enterPoint.position + sittingOffSet, transitionSpeed);
            player.rotation = Quaternion.Slerp(player.rotation, enterPoint.rotation, transitionSpeed);
            
            vehicleController.enabled = true;
            Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            vehicleController.Move(move * Time.deltaTime * movementSpeed);
            
            if (player.position == enterPoint.position + sittingOffSet)
            {
                isInTransition = false;
                vehicleActive = true;
            }
        }

        void Exit()
        {
            player.position = Vector3.Lerp(player.position, exitPoint.position, transitionSpeed);

            if (player.position == exitPoint.position)
            {
                isInTransition = false;
                vehicleActive = false;
            }

            player.GetComponent<BoxCollider>().enabled = true;
            player.GetComponent<Rigidbody>().isKinematic = false;
            player.GetComponent<Player>().controller.enabled = true;
            
        }

    }
}
