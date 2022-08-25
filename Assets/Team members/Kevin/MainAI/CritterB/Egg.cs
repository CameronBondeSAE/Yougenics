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
            StartCoroutine(Hatch());
        }

        IEnumerator Hatch()
        {
            yield return new WaitForSeconds(hatchTimer);
            Destroy(gameObject);
        }

        private void OnDisable()
        {
            Instantiate(critterPrefab, transform.position, Quaternion.identity);
        }
    }
}

