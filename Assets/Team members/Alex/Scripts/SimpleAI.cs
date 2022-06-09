using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Alex
{

    public class SimpleAI : MonoBehaviour
    {
        //Health health;
        //Energy myEnergy;
        public float movementSpeed;
        public float lookingSpeed = 1f;
        public Transform Target;
        public Alex.Food myFoodTarget;
        public List<Alex.Food> FoodList = new List<Alex.Food>();
        Energy myEnergy;
        Rigidbody rb;
        public AIStates currentState;


        public enum AIStates
        {
            LookingForFood,
            Sleeping,
            Wondering,

        }

        // Start is called before the first frame update

        void Start()
        {
            rb = GetComponent<Rigidbody>();
            GetComponent<Energy>().NoEnergyEvent += NoEnergy;
            myEnergy = GetComponent<Energy>();
        }

        // Update is called once per frame
        void Update()
        {

            if (currentState == AIStates.LookingForFood)
            {
                LookForFood();
                MoveToFood();
            }
            else if (currentState == AIStates.Wondering)
            {
                WonderingAround();
            }
            else if (currentState == AIStates.Sleeping)
            {
                Sleeping();
            }
                    

            

            //MoveToFood();
            //LookForFood();
        }

        public void WonderingAround()
        {
            if (myEnergy.energyAmount >= 50 && myEnergy.energyAmount < 80)
            {
                rb.AddRelativeForce(movementSpeed, 0, movementSpeed);
            }
        }

        public void Sleeping()
        {
            if (myEnergy.energyAmount >= 80)
            {
                rb.velocity = Vector3.zero;
            }
        }


        public void LookForFood()
        {
            myFoodTarget = FindObjectOfType<Alex.Food>();
            if (myFoodTarget != null)
            {
                Target = myFoodTarget.transform;
                transform.LookAt(Target);
            }




            /*
            foreach (Food item in FindObjectsOfType<Food>())
                {
                transform.LookAt(FoodList);

                var nearestDist = float.MaxValue;
                Food nearestObj = null;

                nearestObj = FoodListTest;

                FindObjectOfType<Food>();



                FindFood();
                }
            */


        }

        public void MoveToFood()
        {
            if (myEnergy.energyAmount <= 20 && Target != null)
            {
                //print("Low energy find food");
                rb.AddRelativeForce(0, 0, movementSpeed);
            }
            /*else
            {
                rb.velocity = Vector3.zero;
            }
            */
        }


        public void NoEnergy()
        {
            if (myEnergy.energyAmount <= 0)
            {
                print("Hp loss he");
                //FindObjectOfType<Health>().Hp += -1;
            }
        }


        /*
        public void NoEnergy(Energy AIHasNoEnergy)
        {


            if (AIHasNoEnergy.energyAmount == 0)
            {
                FindObjectOfType<hp>() -= 1;
                print("HP loss here");
            }


        }
        */
    }

}
