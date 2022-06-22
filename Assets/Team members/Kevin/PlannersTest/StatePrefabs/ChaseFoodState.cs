using System.Collections;
using System.Collections.Generic;
using Anthill.AI;
using UnityEngine;

namespace Kevin
{
    public class ChaseFoodState : AntAIState
    {
        public GameObject gluttonPrefab;
        public GluttonBase gluttonBase;
        public override void Create(GameObject aGameObject)
        {
            base.Create(aGameObject);
            gluttonPrefab = aGameObject;
            gluttonBase = gluttonPrefab.GetComponent<GluttonBase>();
        }
    }
}