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
        //public bool vehicleActive;
        //public Vehicle vehicle;
        

        public Minh.Health health;
        public HealthBarAlex healthBarAlex;
        public EnergyBarAlex energyBarAlex;
        public Energy energy;
        public TaskList taskList;

        // Start is called before the first frame update
        void Start()
        {
            //GetComponent<Vehicle>().vehicleActive = false;
            controller = gameObject.AddComponent<CharacterController>();
            Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            controller.Move(move * Time.deltaTime * movementSpeed);

            energyBarAlex.SetMaxEnergy(energy.energyAmount);
            healthBarAlex.SetMaxHealth(health.Hp);
            taskList.ToggleTest();

        }
        
        
        void TakeDamage(int damage)
        {
            health.Hp -= damage;
                
            healthBarAlex.SetHealth(health.Hp);
        }
        
        void Update()
        {

            if (Input.GetKeyDown(KeyCode.Space))
            {
                TakeDamage(20);
            }


            
            
            //If player is not in a vehicle they have regular movements 
            //if (GetComponent<Vehicle>().vehicleActive == false)
            {
                Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
                controller.Move(move * Time.deltaTime * movementSpeed);
            }
            /*
            //If player is in vehicle player movements are disabled 
            if (vehicle.vehicleActive == true)
            {
                controller.enabled = false;
            }
            */
        }
    }
}
