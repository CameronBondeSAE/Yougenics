using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kevin
{
    public class MatingRange : MonoBehaviour
    {
        public GameObject foxObject;
        public FoxModel _foxModel;
        public Vector3 offSet = new Vector3(1f, 1f, 1f);
        public void Start()
        {
            _foxModel = foxObject.GetComponent<FoxModel>();
            this.GetComponent<SphereCollider>().radius = _foxModel.matingRange; 
        }

        public void OnTriggerEnter(Collider other)
        {
            INpc npc = other.GetComponent<INpc>();
            if (npc != null)
            {
                _foxModel.isMating = true;
                _foxModel.entityInSightRange = false;
                StartCoroutine(Breed());

            }
        }

        IEnumerator Breed()
        {
            yield return new WaitForSeconds(5f);
            Instantiate(_foxModel, transform.position - offSet,Quaternion.identity);
        }

        void OnTriggerExit(Collider other)
        {
            INpc npc = other.GetComponent<INpc>();
            if (npc != null)
            {
                _foxModel.isMating = false;
                _foxModel.entityInSightRange = false;
            }
        }
    }
}

