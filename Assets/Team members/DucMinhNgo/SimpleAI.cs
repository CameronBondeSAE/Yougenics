using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minh
{
    public class SimpleAI : MonoBehaviour
    {
        public Food target;
        public Rigidbody rb;
        public float speed = 1000;
        public event Action Foodfinding;
        public Minh.Basestate currentState;


        // Start is called before the first frame update
        void Update()
        {
            Findfood();
        }

        void Findfood()
        {
            target = FindObjectOfType<Food>();



            if (target != null)
            {
                //only move toward target if it exist
                transform.LookAt(target.transform);
                rb.AddRelativeForce(0, 0, speed);
            }
            else
            {
                ChangeState(GetComponent<RestState>());
            }

            Foodfinding?.Invoke();
        }

        public void ChangeState(Minh.Basestate newState)
        {
            // Check if the state is the same and DON'T swap
            if (newState == currentState)
            {
                return;

            }
            if (currentState != null)
            {
                currentState.enabled = false;
            }

            newState.enabled = true;

            // New state swap over to incoming state
            currentState = newState;


        }
    }
}