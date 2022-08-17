using System.Collections;
using UnityEngine;


namespace Alex
{

    public class TreeDropsFood : MonoBehaviour
    {
        Energy energy;
        public GameObject foodPrefab;
        public GameObject treePrefab;
        public Spawner spawner;
        public bool canSpawn;
        public GameObject map;
        Terrain myTerrain;

        private Vector3 tree;
        public GameObject treeTop;

        void Start()
        {

            canSpawn = true;
            energy = GetComponent<Energy>();
            StartCoroutine(SpawnThisTree());
        }

        // Update is called once per frame
        void Update()
        {
            if (energy.EnergyAmount.Value <= 40)
                StartCoroutine(SpawnThisTree());

        }

        public IEnumerator SpawnThisTree()
        {
            if (canSpawn)
                spawner.SpawnSingle(foodPrefab,new Vector3(treeTop.transform.localPosition.x , treeTop.transform.localPosition.y - 3f,
                treeTop.transform.localPosition.z), Quaternion.identity);
            canSpawn = false;
            yield return new WaitForSeconds(2f);
            StopCoroutine(SpawnThisTree());
        }
       
    }
    
    
}
