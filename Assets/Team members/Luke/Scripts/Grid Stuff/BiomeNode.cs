using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Luke
{
	public class BiomeNode : NodeBase
	{
		#region Fields
		
		public BiomeManager Bm;
		
		public BiomeGroundTypes groundType;
		private float fertility;
		private float temperature;

		public Vector2Int iterators = new (0,0);
		public float blueness = 0;
		
		#endregion
		
		#region Properties
		
		public float Fertility
		{
			get => fertility;
			set
			{
				fertility = value;
				CalculateTemperature();
			}
		}
		
		public float Temperature
		{
			get;
		}
		
		#endregion

		//Could add in something to do with time of day.
		public void CalculateTemperature()
		{
			switch (groundType)
			{
				case BiomeGroundTypes.Alpine:
				{
					temperature = 25f - worldPosition.y + fertility*5f;
					break;
				}
				case BiomeGroundTypes.Tundra:
				{
					temperature = 20f + fertility*5f;
					break;
				}
				case BiomeGroundTypes.Field:
				{
					temperature = 25f + fertility * 5f;
					break;
				}
				case BiomeGroundTypes.Forest:
				{
					temperature = 25f + fertility * 15f;
					break;
				}
				case BiomeGroundTypes.Desert:
				{
					temperature = 45f - fertility * 15f;
					break;
				}
			}
		}
		
		public void SpreadFertility()
		{
			/*if (neighbours[iterators.x, iterators.y] == null) return;
			blueness = 1;
			BiomeNode node = (BiomeNode) neighbours[iterators.x, iterators.y];
			
			if (fertility <= node.Fertility)
			{
				node.Fertility -= (1-fertility) * 0.1f;
			}
			else
			{
				node.Fertility += fertility * 0.12f;
			}

			blueness = 0;*/
		}
	}

	public enum BiomeGroundTypes
	{
		Alpine,
		Tundra,
		Field,
		Forest,
		Desert
	}
}