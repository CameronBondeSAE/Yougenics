using System;
using System.Collections;
using System.Collections.Generic;
using NodeCanvas.Tasks.Actions;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

//using Random = System.Random;

namespace Maya
{
    public class Creature : MonoBehaviour
    {
        public Energy energyScript;
        public Food foodObj;
        public Rigidbody myRB;
        public Vector3 targetPos;
        public float rotateSpeed;
        public float moveSpeed;
        private float defaultMoveSpeed = 0.5f;
        private float defaultDrainSpeed = 1;
        public bool isHungry;
        public bool atFood;
        public bool isMoving;


        public void Wander()
        {
            targetPos = new Vector3(Random.Range(transform.position.x, transform.position.x + 50), 0,
                Random.Range(transform.position.z, transform.position.z + 50));
            MoveToTarget();
        }

        public void CheckEnergy()
        {
            if (energyScript.energyAmount <= energyScript.energyMax * 0.5f)
            {
                isHungry = true;
            }
            else
            {
                isHungry = false;
            }
        }

        public bool FindFood()
        {
            RaycastHit hit;
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward * 100));
            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, 30))
            {
                if (hit.collider.GetComponent<Food>() != null)
                {
                    targetPos = hit.point;
                    moveSpeed = defaultMoveSpeed;
                    isMoving=true;
                    return true;
                }
            }
            else
            {
                Wander();
            }

            return false;

        }
        

        public void MoveToTarget()
        {
            transform.LookAt(targetPos);
            if (isMoving)
            {
                myRB.AddForce(Vector3.forward * moveSpeed);
            }
            else
            {
                myRB.velocity.Set(0,0,0);
            }

        }

        private void OnCollisionEnter(Collision other)
        {
            isMoving = false;
            if (other.collider.GetComponent<Food>() != null)
            {
                isMoving = false;
                atFood = true;
            }
            else
            {
                atFood = false;
            }
        }


        public void EatFood()
        {
            if (atFood)
            {
                energyScript.energyAmount += foodObj.energyValue;
            }
        }

        
    }
}
    
