using System.Collections;
using System.Collections.Generic;
using Anthill.AI;
using UnityEngine;
using UnityEngine.AI;

namespace Maya
{ 
    public class GoToFoodState : AntAIState
    {
        public Vision myVision;
        private NavMeshAgent agent;

        public override void Create(GameObject aGameObject)
        {
            base.Create(aGameObject);
            myVision = aGameObject.GetComponent<Vision>();
        }

        public override void Enter()
        {
            base.Enter();
            agent = GetComponentInParent<NavMeshAgent>();
            agent.speed = 10;
        }

        public override void Execute(float aDeltaTime, float aTimeScale)
        {
            base.Execute(aDeltaTime, aTimeScale);
            if (FindFoodState.foodIWant != null)
            {
                agent.SetDestination(FindFoodState.foodIWant[0].transform.position);
            }
            else
            {
                Vector3 newPos = WanderState.RandomNavSphere(transform.position, myVision.GetComponent<SphereCollider>().radius, -1);
                agent.SetDestination(newPos);
            }
        }

        public override void Exit()
        {
            base.Exit();
        }
    }
}