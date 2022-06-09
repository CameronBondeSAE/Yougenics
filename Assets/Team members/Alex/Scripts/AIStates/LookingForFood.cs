using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Alex
{
    public class LookingForFood : StateBase
    {
        Rigidbody rb;
        public float movementSpeed = 5f;
        public Transform Target;
        public Food myFoodTarget;

        private void OnEnabled()
        {
            Debug.Log("Looking for food");
        }

        private void OnDisable()
        {
            rb.velocity = Vector3.zero;
        }

        private void Update()
        {

            rb = GetComponent<Rigidbody>();
            myFoodTarget = FindObjectOfType<Alex.Food>();
            if (myFoodTarget != null)
            {
                Target = myFoodTarget.transform;
                transform.LookAt(Target);
                
            }
            rb.AddRelativeForce(0, 0, movementSpeed);
        }

        

}
}
