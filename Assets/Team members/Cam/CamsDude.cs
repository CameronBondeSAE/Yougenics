using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = System.Object;

namespace Cam
{
    public class Wheel
    {
        
    }
    
    
    public class CamsDude : MonoBehaviour, IEdible
    {
        public Transform target;

        public StateManager stateManager;
        
        void Start()
        {
            stateManager = GetComponent<StateManager>();
            
            // I am interested in this event, so I 'subscribe/listen/observe' to the event
            // GetComponent<Health>().DeathEvent += CamSuperDeath;
            GetComponent<Energy>().FullEnergyEvent += Hyper;
            GetComponent<Energy>().NoEnergyEvent += FindFood;
            GetComponent<Minh.Health>().DeathEvent += CamSuperDeath;
            
            // Initialise states
            stateManager.ChangeState(GetComponent<DiscoState>());
        }


        public GameObject myGo;
        public CamSmash   camSmash;

        public bool       isHealthy;
        public bool       hasFood;

        private void Update()
        {
            
            if (GetComponent<Energy>().energyAmount < 10)
            {
                // Sleep
                stateManager.ChangeState(GetComponent<SleepingState>());
            }
            else if (GetComponent<Energy>().energyAmount > 50)
            {
                // Disco
                stateManager.ChangeState(GetComponent<DiscoState>());
            }
        }

        void FindFood()
        {
            Debug.Log("Find food");
        }

        void Hyper()
        {
            GetComponent<Renderer>().material.color = Color.red;
        }


        public bool CheckThing()
        {
            return true;
        }

        public void DoThing()
        {
            Debug.Log("I did a thing");
            GetComponent<Renderer>().material.color = Color.green;
        }

        public void DoMoreThing(bool what)
        {
            Debug.Log("What = " + what);
        }

        public void CamSuperDeath()
        {
            // Play crazy sound
            // Animate
            // Wait for 5 seconds
            Destroy(gameObject);
        }

        public float GetEnergyAmount()
        {
            return GetComponent<Energy>().energyAmount;
        }

        public float EatMe(float energyRemoved)
        {
            return 0;
        }
    }
}