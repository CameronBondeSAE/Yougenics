using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using Random = UnityEngine.Random;

namespace Luke
{
	public class SpawnFood : MonoBehaviour
	{
		[SerializeField]
		private Bounds bounds = new Bounds();
		
		[SerializeField]
		private List<GameObject> foodPrefabs;

		private List<Transform> worldFoods;

		[SerializeField]
		private float worldEnergy;

		[SerializeField]
		private float maxWorldEnergy = 10000f;

		[SerializeField]
		private float spawningDelayPeriod = 3f;

		private void RemoveFoodFromList(Transform _transform)
		{
			if (worldFoods.Contains(_transform))
			{
				worldFoods.Remove(_transform);
				worldEnergy += _transform.GetComponent<Food>().maxHealth;
			}
		}

		private void SortFoodList()
		{
			foodPrefabs.Sort((x,y) => x.GetComponent<Food>().maxHealth.CompareTo(y.GetComponent<Food>().maxHealth));
		}

		private IEnumerator SpawnFoodLoop()
		{
			for (int i = foodPrefabs.Count; i > 0; i--)
			{
				float energyValue = foodPrefabs[i - 1].GetComponent<Food>().maxHealth;
				if (energyValue <= worldEnergy)
				{
					worldEnergy -= energyValue;
					GameObject go = Instantiate(foodPrefabs[i - 1], transform);
					Vector3 spawnLocation = new Vector3(Random.Range(bounds.center.x-bounds.extents.x*0.5f, bounds.center.x+bounds.extents.x*0.5f),
						bounds.center.y,
						Random.Range(bounds.center.z-bounds.extents.z*0.5f, bounds.center.z+bounds.extents.z*0.5f));
					go.transform.position = spawnLocation;
					go.GetComponent<Food>().RemoveFromListEvent += RemoveFoodFromList;
					break;
				}
			}

			yield return new WaitForSeconds(spawningDelayPeriod);
			StartCoroutine(SpawnFoodLoop());
		}

		// Start is called before the first frame update
		void Start()
		{
			worldEnergy = maxWorldEnergy;
			SortFoodList();
			StartCoroutine(SpawnFoodLoop());
		}

		private void OnDrawGizmosSelected()
		{
			Gizmos.color = new Color(0.5f, 0.5f, 0.5f, 0.5f);
			Gizmos.DrawCube(bounds.center, bounds.extents);
		}
	}
}