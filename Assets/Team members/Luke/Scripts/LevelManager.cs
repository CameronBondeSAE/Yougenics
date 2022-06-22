using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Luke
{
	public class LevelManager : MonoBehaviour
	{
		public Node[,] gridNodeReferences;
		public Vector3 gridTileHalfExtents = new (0.5f ,0.5f, 0.5f);
		
		[SerializeField]
		private Bounds bounds = new Bounds();
		
		[SerializeField]
		private List<GameObject> foodPrefabs;

		private List<Transform> worldFoods = new ();

		[SerializeField]
		private float worldEnergy;

		[SerializeField]
		private float maxWorldEnergy = 10000f;

		[SerializeField]
		private float spawningDelayPeriod = 3f;

		private void ScanWorld()
		{
			for (int x = 0; x < Mathf.RoundToInt(bounds.extents.x); x++)
			{
				for (int z = 0; z < Mathf.RoundToInt(bounds.extents.z); z++)
				{
					gridNodeReferences[x, z] = new Node();
					if (Physics.OverlapBox(new Vector3(x * bounds.extents.x, 0, z * bounds.extents.z),
						    gridTileHalfExtents,  Quaternion.identity) != null)
					{
						// Something is there
						gridNodeReferences[x, z].isBlocked = true;
					}
				}
			}
		}

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
					worldFoods.Add(go.transform);
					Vector3 spawnLocation = new Vector3(Random.Range(bounds.center.x-bounds.extents.x*0.5f+1, bounds.center.x+bounds.extents.x*0.5f-1),
						bounds.center.y,
						Random.Range(bounds.center.z-bounds.extents.z*0.5f+1, bounds.center.z+bounds.extents.z*0.5f-1));
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
			gridNodeReferences = new Node[Mathf.RoundToInt(bounds.extents.x),Mathf.RoundToInt(bounds.extents.z)];
			ScanWorld();
		}

		private void OnDrawGizmosSelected()
		{
			Gizmos.color = new Color(0.5f, 0.5f, 0.5f, 0.5f);
			Gizmos.DrawCube(bounds.center, bounds.extents);
		}
	}
}