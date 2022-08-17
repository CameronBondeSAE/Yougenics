using System.Collections;
using System.Collections.Generic;
using Anthill.AI;
using UnityEngine;

namespace Kevin
{
    public class CritterAAIState : AntAIState
    {
        protected CritterA critterA;
        public override void Create(GameObject aGameObject)
        {
            base.Create(aGameObject);

            critterA = aGameObject.GetComponent<CritterA>();
        }
        
       
    }
}

