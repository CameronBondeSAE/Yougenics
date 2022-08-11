using System.Collections;
using System.Collections.Generic;
using Anthill.AI;
using NodeCanvas.Tasks.Actions;
using UnityEngine;

namespace Kevin
{
    public class HuntingState : AntAIState
    {
        public GameObject critterAPrefab;
        public CritterA critterA;
        public override void Create(GameObject aGameObject)
        {
            base.Create(aGameObject);
            critterAPrefab = aGameObject;
            critterA = critterAPrefab.GetComponent<CritterA>();
        }
        
        public override void Execute(float aDeltaTime, float aTimeScale)
        {
            //base.Execute();
            Debug.Log("Hunting");
        }
        
    }
}