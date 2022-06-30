using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Luke
{
	public class WorldFlooder : MonoBehaviour
	{
		#region Variables
		
		[SerializeField]
		private Bounds bounds;

		public FloodingNode[,] gridNodeReferences;
		public Vector3 gridTileHalfExtents = new (0.5f ,0.5f, 0.5f);
		private int worldSizeX;
		private int worldSizeZ;
		private int worldEdgeX;
		private int worldEdgeZ;
		int[] centreCoords;
		private bool centreSet;

		[SerializeField]
		private List<GameObject> foodPrefabs;

		private List<Transform> worldFoods = new ();

		[SerializeField]
		private float worldEnergy;

		[SerializeField]
		private float maxWorldEnergy = 10000f;

		[SerializeField]
		private float spawningDelayPeriod = 3f;
		
		#endregion
		
		#region World Gridding
		public void ScanWorld()
		{
			for (int x = 0; x < worldSizeX; x++)
			{
				for (int z = 0; z < worldSizeZ; z++)
				{
					gridNodeReferences[x, z] = new FloodingNode() {WF = this};
					if (Physics.OverlapBox(new Vector3(worldEdgeX + x, 0, worldEdgeZ + z),
						    gridTileHalfExtents, Quaternion.identity, 251).Length != 0)
					{
						gridNodeReferences[x, z].isBlocked = true;
					}
				}
			}
			IntroduceNeighbours();
		}

		private void IntroduceNeighbours()
		{
			for (int x = 0; x < worldSizeX; x++)
			{
				for (int z = 0; z < worldSizeZ; z++)
				{
					if (x > 0) gridNodeReferences[x-1, z].neighbours[2,1] = gridNodeReferences[x, z];
					if (z > 0) gridNodeReferences[x, z-1].neighbours[1,2] = gridNodeReferences[x, z];
					if (x < worldSizeX-1) gridNodeReferences[x+1, z].neighbours[0,1] = gridNodeReferences[x, z];
					if (z < worldSizeZ-1) gridNodeReferences[x, z+1].neighbours[1,0] = gridNodeReferences[x, z];
					if (x > 0 && z > 0) gridNodeReferences[x-1, z-1].neighbours[2,2] = gridNodeReferences[x, z];
					if (x > 0 && z < worldSizeZ-1) gridNodeReferences[x-1, z+1].neighbours[2,0] = gridNodeReferences[x, z];
					if (x < worldSizeX-1 && z > 0) gridNodeReferences[x+1, z-1].neighbours[0,2] = gridNodeReferences[x, z];
					if (x < worldSizeX-1 && z < worldSizeZ-1) gridNodeReferences[x+1, z+1].neighbours[0,0] = gridNodeReferences[x, z];
				}
			}
		}

		public void FillWorld()
		{
			StopAllCoroutines();
			
			centreCoords = new int[2];
			centreCoords[0] = Random.Range(0, worldSizeX);
			centreCoords[1] = Random.Range(0, worldSizeZ);
			int breaker = 0;
			while (gridNodeReferences[centreCoords[0], centreCoords[1]].isBlocked && breaker < 100)
			{
				centreCoords[0] = Random.Range(0, worldSizeX);
				centreCoords[1] = Random.Range(0, worldSizeZ);
				breaker++;
			}
			if (breaker > 99)
			{
				Debug.Log("Too many blocked nodes!");
				return;
			}

			centreSet = true;
			gridNodeReferences[centreCoords[0], centreCoords[1]].isCentre = true;
			gridNodeReferences[centreCoords[0], centreCoords[1]].FillAmount = 1;
			StartCoroutine(LoopFillNode(gridNodeReferences[centreCoords[0], centreCoords[1]]));
		}

		public void StartFillLoop(FloodingNode floodingNode)
		{
			StartCoroutine(LoopFillNode(floodingNode));
		}
		
		private IEnumerator LoopFillNode(FloodingNode floodingNode)
		{
			floodingNode.FillSelfAndNeighbours(0.2f);

			yield return new WaitForSeconds(0.1f);
			
			if (floodingNode.FillAmount < 1) StartCoroutine(LoopFillNode(floodingNode));
		}
		
		#endregion

		#region Food Spawning
		
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
		
		#endregion

		// Start is called before the first frame update
		void Start()
		{
			worldEnergy = maxWorldEnergy;
			SortFoodList();
			StartCoroutine(SpawnFoodLoop());
			worldSizeX = Mathf.RoundToInt(bounds.extents.x) + 1;
			worldSizeZ = Mathf.RoundToInt(bounds.extents.z) + 1;
			worldEdgeX = Mathf.RoundToInt(bounds.center.x-bounds.extents.x/2);
			worldEdgeZ = Mathf.RoundToInt(bounds.center.z-bounds.extents.z/2);
			gridNodeReferences = new FloodingNode[worldSizeX,worldSizeZ];
			ScanWorld();
		}

		void Update()
		{
			if (centreSet) gridNodeReferences[centreCoords[0], centreCoords[1]].FillSelfAndNeighbours(0.2f);
		}

		private void OnDrawGizmosSelected()
		{
			Gizmos.color = new Color(0.5f, 0.5f, 0.5f, 0.5f);
			Gizmos.DrawCube(bounds.center, bounds.extents);
			
			for (int x = 0; x < worldSizeX; x++)
			{
				for (int z = 0; z < worldSizeZ; z++)
				{
					FloodingNode floodingNode = gridNodeReferences[x, z];
					if (floodingNode.isBlocked)
					{
						Gizmos.color = Color.red;
						Gizmos.DrawCube(new Vector3(worldEdgeX+x, 0, worldEdgeZ+z), Vector3.one);
					}
					else if (floodingNode.isCentre)
					{
						Gizmos.color = new Color(1-floodingNode.FillAmount, floodingNode.FillAmount, 1);
						Gizmos.DrawCube(new Vector3(worldEdgeX+x, 0, worldEdgeZ+z), Vector3.one);
					}
					else
					{
						Gizmos.color = new Color(1-floodingNode.FillAmount, floodingNode.FillAmount, 1-floodingNode.FillAmount);
						Gizmos.DrawCube(new Vector3(worldEdgeX+x, 0, worldEdgeZ+z), Vector3.one);
					}
				}
			}
		}
	}
}