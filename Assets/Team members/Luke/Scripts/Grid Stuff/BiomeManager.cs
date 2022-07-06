using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Luke
{
	public class BiomeManager : MonoBehaviour
	{

		#region Variables

		private BiomeNode[,] BiomeNodes;

		[SerializeField] private Vector3 gridTileSize;
		[SerializeField] private Vector2 gridSize;
		[SerializeField] private Vector3 gridOrigin;

		public Vector2Int numberOfTiles;
		public Vector2Int iterators = new (0,0);

		[SerializeField] private Vector2 perlinOffsets;
		[SerializeField] private Vector2 perlinWavelengths;

		[SerializeField] private Collider terrainCollider;

		#endregion
		
		#region Methods
		
		public void FillGrid()
		{
			numberOfTiles = new Vector2Int(Mathf.RoundToInt(gridSize.x / gridTileSize.x),
				Mathf.RoundToInt(gridSize.y / gridTileSize.z));
			BiomeNodes = new BiomeNode[numberOfTiles.x, numberOfTiles.y];
			for (int x = 0; x < numberOfTiles.x; x++)
			{
				for (int y = 0; y < numberOfTiles.y; y++)
				{
					float xCoord = (float) x / numberOfTiles.x;
					float yCoord = (float) y / numberOfTiles.y;
					
					//or should I just get the terrain height directly?

					/*Vector3 raycastOrigin = new(gridOrigin.x + x * gridTileSize.x, gridOrigin.y + 200f,
						gridOrigin.z + y * gridTileSize.z);
					terrainCollider.Raycast(new Ray(raycastOrigin, Vector3.down), out RaycastHit hitInfo, 400f);
					float altitude = hitInfo.collider.transform.position.y;*/
					BiomeNodes[x, y] = new BiomeNode
					{
						worldPosition = new Vector3(gridOrigin.x + x * gridTileSize.x, /*altitude*/gridTileSize.y,
							gridOrigin.z + y * gridTileSize.z),
						Fertility = Mathf.PerlinNoise(xCoord*perlinWavelengths.x+perlinOffsets.x,yCoord*perlinWavelengths.y+perlinOffsets.y),
						Bm = this
					};
				}
			}
			IntroduceNeighbours();
		}
		
		private void IntroduceNeighbours()
		{
			for (int x = 0; x < numberOfTiles.x; x++)
			{
				for (int y = 0; y < numberOfTiles.y; y++)
				{
					if (x > 0) BiomeNodes[x-1, y].neighbours[2,1] = BiomeNodes[x, y];
					if (y > 0) BiomeNodes[x, y-1].neighbours[1,2] = BiomeNodes[x, y];
					if (x < numberOfTiles.x-1) BiomeNodes[x+1, y].neighbours[0,1] = BiomeNodes[x, y];
					if (y < numberOfTiles.y-1) BiomeNodes[x, y+1].neighbours[1,0] = BiomeNodes[x, y];
					if (x > 0 && y > 0) BiomeNodes[x-1, y-1].neighbours[2,2] = BiomeNodes[x, y];
					if (x > 0 && y < numberOfTiles.y-1) BiomeNodes[x-1, y+1].neighbours[2,0] = BiomeNodes[x, y];
					if (x < numberOfTiles.x-1 && y > 0) BiomeNodes[x+1, y-1].neighbours[0,2] = BiomeNodes[x, y];
					if (x < numberOfTiles.x-1 && y < numberOfTiles.y-1) BiomeNodes[x+1, y+1].neighbours[0,0] = BiomeNodes[x, y];
				}
			}
		}
		
		#endregion

		#region IEnumerators

		private IEnumerator SpreadFertilityWorld()
		{
			StartCoroutine(SpreadFertilityNode(BiomeNodes[iterators.x, iterators.y]));
			
			yield return new WaitForEndOfFrame();

			if (!(++iterators.x < BiomeNodes.GetLength(0)))
			{
				iterators.x = 0;
				if (!(++iterators.y < BiomeNodes.GetLength(1)))
				{
					iterators.y = 0;
				}
			}

			StartCoroutine(SpreadFertilityWorld());
		}
		
		private IEnumerator SpreadFertilityNode(BiomeNode node)
		{
			if (node.iterators.magnitude == 1)
			{
				yield return null;
			}
			else
			{
				node.SpreadFertility();
				yield return new WaitForEndOfFrame();
			}

			if (!(++node.iterators.x < 3))
			{
				node.iterators.x = 0;
				if (!(++node.iterators.y < 3))
				{
					node.iterators.y = 0;
				}
			}
			
			if (node.iterators.magnitude != 0) StartCoroutine(SpreadFertilityNode(node));
		}
		
		#endregion
		
		// Start is called before the first frame update
		void Start()
		{
			perlinOffsets = new Vector2(Random.Range(0f, 255f), Random.Range(0f,255f));
			FillGrid();
			StartCoroutine(SpreadFertilityWorld());
		}

		private void OnDrawGizmosSelected()
		{
			for (int x = 0; x < numberOfTiles.x; x++)
			{
				for (int y = 0; y < numberOfTiles.y; y++)
				{
					Gizmos.color = new Color(1-BiomeNodes[x,y].Fertility,0.5f+BiomeNodes[x,y].Fertility/2f,0,0.5f);
					Gizmos.DrawCube(BiomeNodes[x,y].worldPosition, gridTileSize);
				}
			}
		}
	}
}