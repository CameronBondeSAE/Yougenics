using System.Collections;
using System.Collections.Generic;
using Anthill.AI;
using Tanks;
using UnityEngine;

namespace Ollie
{
    public class Wander : AntAIState
    {
        public Vector3 targetLocation;
        public float moveTime;
        public float moveSpeed;
        private bool interactingTarget;
        private GameObject parent;
        
        public override void Create(GameObject aGameObject)
        {
            parent = aGameObject;
        }

        public override void Enter()
        {
            base.Enter();
            moveTime = 5;
            moveSpeed = 5;
        }

        public override void Execute(float aDeltaTime, float aTimeScale)
        {
            //StandardMovement();
        }

        public override void Exit()
        {
            base.Exit();
        }

        public override void Destroy(GameObject aGameObject)
        {
            base.Destroy(aGameObject);
        }
        
        public void StandardMovement()
        {
            parent.transform.position =
                Vector3.MoveTowards(parent.transform.position, targetLocation, (moveSpeed*Time.deltaTime));
            
            if (parent.transform.position == targetLocation && !interactingTarget)
            {
                StartCoroutine(RandomLocation());
            }
        }

        public IEnumerator RandomLocation()
        {
            targetLocation = new Vector3((UnityEngine.Random.Range(-40,40)), 1, (UnityEngine.Random.Range(-40,40)));
            yield return new WaitForSeconds(moveTime);
            print("noLocationSet = true");
        }
        
    }
}