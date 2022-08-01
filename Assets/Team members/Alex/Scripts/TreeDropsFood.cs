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
            spawner.SpawnSingle(treePrefab,new Vector3(treeTop.transform.localPosition.x - 20f, treeTop.transform.localPosition.y,
                treeTop.transform.localPosition.z), Quaternion.identity);
            canSpawn = false;
            energy = GetComponent<Energy>();
            
        }

        void Awake()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
            if (GetComponent<Energy>().EnergyAmount.Value <= 40)
                spawner.SpawnSingle(foodPrefab, new Vector3(Random.Range(treeTop.transform.localPosition.x -10, treeTop.transform.localPosition.x +10), treeTop.transform.position.y - 4f, Random.Range(treeTop.transform.localPosition.x -10, treeTop.transform.localPosition.x +10)), Quaternion.identity);
        }
    }
    
    
}
