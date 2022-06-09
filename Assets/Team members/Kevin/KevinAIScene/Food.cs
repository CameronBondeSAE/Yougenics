using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kevin
{
    public class Food : MonoBehaviour
    {
        public float healthValue;
        public GameObject foxObject;
        public FoxModel _foxModel;
        public void Awake()
        {
            _foxModel = foxObject.GetComponent<FoxModel>();
        }

        public void OnTriggerEnter(Collider other)
        {
            IEntity entity = other.GetComponent<IEntity>();
            if (entity != null)
            {
                _foxModel.healthPoints += healthValue;
                StartCoroutine(Eaten());
            }
        }

        IEnumerator Eaten()
        {
            yield return new WaitForSeconds(1f);
            //_foxModel.surroundingEntities.RemoveAt(0);
            Destroy(gameObject);
            _foxModel.surroundingEntities.Clear();
            _foxModel.entityInAttackRange = false; 
            _foxModel.entityInSightRange = false;
        }
    } 
}

