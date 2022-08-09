using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem.XInput;
using Random = UnityEngine.Random;

namespace Luke
{
	public class BiomeManager : MonoBehaviour
	{

		#region Variables

		[SerializeField] private bool breaker;
		[SerializeField] private float updateSpeedSeconds = 1f;

		private BiomeNode[,] BiomeNodes;

		[SerializeField] private int numberOfTiles1D;
		[SerializeField] private Vector3 tileSize;

		//x
		[SerializeField] private float _levelLength;
		//z
		[SerializeField] private float _levelWidth;
		[SerializeField] private float tileHeight;
		[SerializeField] private Vector3 gridOrigin;

		private int[][] _neighbours =
		{
			new [] {-1, -1},
			new [] {0, -1},
			new [] {1, -1},
			new [] {-1, 0},
			new [] {1, 0},
			new [] {-1, 1},
			new [] {0, 1},
			new [] {1, 1}
		};

		[SerializeField] private Vector2 perlinOffsets;
		[SerializeField] private Vector2 perlinWavelengths;

		[SerializeField] private Collider terrainCollider;

		#endregion
		
		#region Methods
		
		public void FillGrid()
		{
			float tileLength = _levelLength/numberOfTiles1D;
			float tileWidth = _levelWidth/numberOfTiles1D;
			tileSize = new Vector3(tileLength, tileHeight, tileWidth);
			BiomeNodes = new BiomeNode[numberOfTiles1D, numberOfTiles1D];
			for (int x = 0; x < numberOfTiles1D; x++)
			{
				for (int y = 0; y < numberOfTiles1D; y++)
				{
					float xCoord = (float) x / numberOfTiles1D;
					float yCoord = (float) y / numberOfTiles1D;
					
					//or should I just get the terrain height directly?

					/*Vector3 raycastOrigin = new(gridOrigin.x + x * gridTileSize.x, gridOrigin.y + 200f,
						gridOrigin.z + y * gridTileSize.z);
					terrainCollider.Raycast(new Ray(raycastOrigin, Vector3.down), out RaycastHit hitInfo, 400f);
					float altitude = hitInfo.collider.transform.position.y;*/
					BiomeNodes[x, y] = new BiomeNode
					{
						worldPosition = new Vector3(gridOrigin.x + x * tileSize.x, /*altitude*/tileSize.y,
							gridOrigin.z + y * tileSize.z),
						indices = new [] {x,y},
						Fertility = Mathf.PerlinNoise(xCoord*perlinWavelengths.x+perlinOffsets.x,yCoord*perlinWavelengths.y+perlinOffsets.y),
						Bm = this
					};
				}
			}
		}
		
		
		
		#endregion

		#region IEnumerators

		private IEnumerator SpreadFertilityWorld()
		{
			int i=0;
			int j=0;
			while (!breaker)
			{
				yield return new WaitForSeconds(updateSpeedSeconds);
				SpreadFertilityNode(BiomeNodes[i,j]);
				if (++i >= numberOfTiles1D)
				{
					i=0;
					if (++j >= numberOfTiles1D)
					{
						j=0;
					}
				}
			}
		}
		
		private void SpreadFertilityNode(BiomeNode node)
		{
			foreach (int[] i in _neighbours)
			{
				int xIndex = node.indices[0] + i[0];
				int yIndex = node.indices[1] + i[1];

				if (xIndex < 0 || xIndex >= numberOfTiles1D || yIndex < 0 || yIndex >= numberOfTiles1D) continue;
				FertilityCalculation(node, BiomeNodes[xIndex, yIndex]);
			}
		}

		private void FertilityCalculation(BiomeNode nodeFrom, BiomeNode nodeTo)
		{
			if (nodeFrom.Fertility <= nodeTo.Fertility)
			{
				nodeTo.Fertility -= (1-nodeFrom.Fertility) * 0.1f;
			}
			else
			{
				nodeTo.Fertility += nodeFrom.Fertility * 0.12f;
			}
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
			for (int x = 0; x < numberOfTiles1D; x++)
			{
				for (int y = 0; y < numberOfTiles1D; y++)
				{
					if (BiomeNodes == null) continue;
					Gizmos.color = new Color(1-BiomeNodes[x,y].Fertility,0.5f+BiomeNodes[x,y].Fertility/2f,0,0.5f);
					Gizmos.DrawCube(BiomeNodes[x,y].worldPosition, tileSize);
				}
			}
		}
	}
}