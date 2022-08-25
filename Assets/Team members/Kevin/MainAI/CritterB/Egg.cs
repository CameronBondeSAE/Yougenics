using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kevin
{
    public class Egg : MonoBehaviour
    {
        public float hatchTimer = 25f;
        public GameObject critterPrefab;
        private void OnEnable()
        {
            StartCoroutine(Birth());
            StartCoroutine(Hatch());
        }

        IEnumerator Birth()
        {
            yield return new WaitForSeconds(hatchTimer - 1f);
            Instantiate(critterPrefab, transform.position, Quaternion.identity);
        }
        
        IEnumerator Hatch()
        {
            yield return new WaitForSeconds(hatchTimer);
            Destroy(gameObject);
        }
    }
}

