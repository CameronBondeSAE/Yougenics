using Minh;
using System.Collections.Generic;
using UnityEngine;

namespace Cam
{
	public enum Allegiances
	{
		Human,
		Luke,
		Ollie,
		Kevin,
		Angelo,
		Cam
	}

	public class CommonAttributes : MonoBehaviour
	{
		public List<Allegiances> allyList;
		public List<Allegiances> enemyList;

		public float dangerLevel;
	}
}