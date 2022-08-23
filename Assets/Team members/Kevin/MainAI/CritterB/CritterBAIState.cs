using System.Collections;
using System.Collections.Generic;
using Anthill.AI;
using UnityEngine;

namespace Kevin
{
    public class CritterBAIState : AntAIState
    {
        protected CritterB critterB;
        public override void Create(GameObject aGameObject)
        {
            base.Create(aGameObject);

            critterB = aGameObject.GetComponent<CritterB>();
        }
        
       
    }
}
