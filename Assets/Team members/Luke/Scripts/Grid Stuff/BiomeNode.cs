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
		private float _fertility;
		private float _temperature;

		#endregion
		
		#region Properties
		
		public float Fertility
		{
			get => _fertility;
			set
			{
				_fertility = value;
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
					_temperature = 25f - worldPosition.y + _fertility*5f;
					break;
				}
				case BiomeGroundTypes.Tundra:
				{
					_temperature = 20f + _fertility*5f;
					break;
				}
				case BiomeGroundTypes.Field:
				{
					_temperature = 25f + _fertility * 5f;
					break;
				}
				case BiomeGroundTypes.Forest:
				{
					_temperature = 25f + _fertility * 15f;
					break;
				}
				case BiomeGroundTypes.Desert:
				{
					_temperature = 45f - _fertility * 15f;
					break;
				}
			}
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