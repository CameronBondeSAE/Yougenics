using System.Collections;
using System.Collections.Generic;
using Ollie;
using Unity.VisualScripting;
using UnityEngine;

namespace Kevin
{
    public class CritterBase : MonoBehaviour, INpc, IEntity
    {
        public float health;

        public float energy;

        public bool isPatrolling;
        public bool isChasing;
        public bool isAttacking;
        public bool isSleeping;
        public bool inVisionRange;
        public bool inAttackRange;
        public bool energyLow;

        public float visionRange, attackRange;

        public void Awake()
        {
            visionRange = this.GetComponent<SphereCollider>().radius;
        }
        public void OnTriggerEnter(Collider other)
        {
            IEntity entity = other.GetComponent<IEntity>();
            if (entity != null)
            {
                isChasing = true;
                transform.localScale = new Vector3(5f, 5f, 5f);
            }
        }

        public void OnTriggerExit(Collider other)
        {
            IEntity entity = other.GetComponent<IEntity>();
            if (entity != null)
            {
                isChasing = false; 
                transform.localScale = new Vector3(1f, 1f, 1f);
            }
        }

        public void Update()
        {
            if (InRange == true)
            {
                isChasing = true;
            }
            else
            {
                isChasing = false;
            }
        }
        private void OnDrawGizmosSelected() 
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, visionRange);
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, attackRange);
        }
        
        public void Patrol()
        {
            Debug.Log("Patrolling");
        }

        private void GenerateWalkPoint()
        {
            
        }

        public void Chase()
        {
            Debug.Log("Chasing");
        }

        public void Attack()
        {
            Debug.Log("Attacking");
        }

        public void Sleep()
        {
            Debug.Log("Sleeping");
        }

        public bool InRange { get; set; }
    }
}


