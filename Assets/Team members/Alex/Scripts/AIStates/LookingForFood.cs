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

        public void OnEnable()
        {
            //Changes colour to blue when entering this state 
            GetComponent<Renderer>().material.color = Color.blue;
        }

        private void OnDisable()
        {
            //Stop movmement when leaving this state 
            rb.velocity = Vector3.zero;
        }

        private void Update()
        {
            //Sets the food as the target and makes the AI look towards its position
            rb = GetComponent<Rigidbody>();
            myFoodTarget = FindObjectOfType<Alex.Food>();
            if (myFoodTarget != null)
            {
                Target = myFoodTarget.transform;
                Target.position = new Vector3(Target.position.x, rb.position.y, Target.position.z);
                transform.LookAt(Target);
                
            }
            //Moves AI 
            rb.AddRelativeForce(0, 0, movementSpeed);
        }

        

}
}
