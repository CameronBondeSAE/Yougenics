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
        public Vector3 overrideLocation;
        public bool overrideLocationCheck;
        public float moveTime;
        public float moveSpeed;
        private bool interactingTarget;
        private GameObject parent;
        public Vector3 controllerPos;

        public override void Create(GameObject aGameObject)
        {
            parent = aGameObject;
        }

        public override void Enter()
        {
            base.Enter();
            moveTime = 5;
            moveSpeed = 5;
            overrideLocationCheck = false;
            Finish();
        }

        public override void Execute(float aDeltaTime, float aTimeScale)
        {
            StandardMovement();
        }

        public override void Exit()
        {
            base.Exit();
            parent.GetComponentInChildren<Controller>().StopMovement();
        }

        public override void Destroy(GameObject aGameObject)
        {
            base.Destroy(aGameObject);
        }
        
        public void StandardMovement()
        {
            if (parent.transform.position == parent.GetComponentInChildren<Controller>().targetLocation)// && !interactingTarget)
            {
                StartCoroutine(RandomLocation());
            }
        }

        public IEnumerator RandomLocation()
        {
            parent.GetComponentInChildren<Controller>().targetLocation = new Vector3((UnityEngine.Random.Range(-40,40)), 1, (UnityEngine.Random.Range(-40,40)));
            yield return new WaitForSeconds(moveTime);
            print("noLocationSet = true");
        }
        
    }
}