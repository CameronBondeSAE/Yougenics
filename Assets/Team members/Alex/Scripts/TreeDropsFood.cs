using System.Collections;
using UnityEngine;


namespace Alex
{

    public class TreeDropsFood : MonoBehaviour
    {
        public Energy energy;
        Food food;
        

        private Vector3 tree;
        public GameObject treeTop;

        // Start is called before the first frame update
        void Start()
        {
            energy = GetComponent<Energy>();
            food = GetComponent<Alex.Food>();
            StartCoroutine(DropFood());
        }

        // Update is called once per frame
        void Update()
        {
            if (energy.EnergyAmount.Value <= 40)
            {
                if(food != null)
                Instantiate(food, new Vector3(treeTop.transform.position.x, treeTop.transform.position.y - 1f, treeTop.transform.position.z), Quaternion.identity);

            }
        }

        public IEnumerator DropFood()
        {
            if (energy.EnergyAmount.Value <= 40)
            {
                yield return new WaitForSeconds(1);
                FoodThatDrops();
            }
        }

        public void FoodThatDrops()
        {
           if (food != null)
                Instantiate(food, new Vector3(treeTop.transform.position.x, treeTop.transform.position.y - 1f, treeTop.transform.position.z), Quaternion.identity);
        }
    }
}
