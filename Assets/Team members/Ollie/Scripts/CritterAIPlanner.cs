using System;
using System.Collections;
using System.Collections.Generic;
using Tanks;
using Unity.VisualScripting;
using UnityEngine;

namespace Ollie
{
    public class CritterAIPlanner : MonoBehaviour
    {
        public List<Vector3> foodLocationList;
        public float moveSpeed;

        #region Bools for planner World State
        [HideInInspector] public bool isSafe;
        [HideInInspector] public bool isHungry;
        [HideInInspector] public bool npcNearby;
        [HideInInspector] public bool preyFound;
        [HideInInspector] public bool foodFound;
        [HideInInspector] public bool predatorFound;
        [HideInInspector] public bool mateFound;
        [HideInInspector] public bool findingPrey;
        [HideInInspector] public bool foodLocated;
        [HideInInspector] public bool findingPredator;
        [HideInInspector] public bool findingMate;
        [HideInInspector] public bool isHorny;
        [HideInInspector] public bool healthLow;
        [HideInInspector] public bool runningAway;
        [HideInInspector] public bool inDanger;
        [HideInInspector] public bool foodNearby;
        [HideInInspector] public bool preyNearby;
        [HideInInspector] public bool mateNearby;
        [HideInInspector] public bool predatorNearby;
        [HideInInspector] public bool preyDead;
        #endregion

        private void Start()
        {
            moveSpeed = 0.25f;
            
            //testing purposes only
            isHungry = true;
            healthLow = true;
        }

        public void AddFoodToList(GameObject foodLocation)
        {
            if (!foodLocationList.Contains(foodLocation.transform.position))
            {
                foodLocationList.Add(foodLocation.transform.position);
            }
        }

        public void SetFoodLocated()
        {
            foodLocated = true;
        }
    }
}