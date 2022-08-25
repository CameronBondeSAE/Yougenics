using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Kevin
{
    public class Egg : MonoBehaviour
    {
        public float hatchTimer = 25f;
        public List<GameObject> critterPrefabs;
        private void OnEnable()
        {
            StartCoroutine(Birth());
            StartCoroutine(Hatch());
        }

        IEnumerator Birth()
        {
            yield return new WaitForSeconds(hatchTimer - 0.5f);
            Instantiate(critterPrefabs[Random.Range(0,2)], transform.position, Quaternion.identity);
        }
        
        IEnumerator Hatch()
        {
            yield return new WaitForSeconds(hatchTimer);
            Destroy(gameObject);
        }
    }
}

