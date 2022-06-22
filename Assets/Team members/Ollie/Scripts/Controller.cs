using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ollie
{
    public class Controller : MonoBehaviour
    {
        public CritterAIPlanner parent;

        public Vector3 targetLocation;

        private void Awake()
        {
            targetLocation = parent.transform.position;
        }

        void FixedUpdate()
        {
            MoveToTarget(targetLocation);
        }

        public void MoveToTarget(Vector3 targetLoc)
        {
            parent.transform.position = Vector3.MoveTowards(parent.transform.position, targetLoc, parent.moveSpeed);
        }

        public void StopMovement()
        {
            targetLocation = parent.transform.position;
        }
    }
}