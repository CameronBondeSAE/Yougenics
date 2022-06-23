using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Luke
{
	public class Node
	{
		public bool isBlocked;
		public bool isCentre;
		public bool isFilling;

		public LevelManager lm;

		private float fillAmount;
		public float FillAmount
		{
			get
			{
				return fillAmount;
			}
			set
			{
				if (value >=1) fillAmount = 1;
				else if (value <= 0) fillAmount = 0;
				else fillAmount = value;
			}
		}
		
		public Node[,] neighbours = new Node[3,3];

		public void FillSelfAndNeighbours(float amount)
		{
			FillAmount += amount/2;
			isFilling = true;

			if (FillAmount < 0.5f)
			{
				FillAmount += amount/2;
				return;
			}

			for (int x = 0; x < 3; x++)
			{
				for (int z = 0; z < 3; z++)
				{
					Node neighbour = neighbours[x, z];
					if (!(neighbour == null || neighbour.isBlocked))
					{
						if (x == 1 || z == 1) neighbour.FillAmount += amount/4;
						else neighbour.FillAmount += amount/10;
						if (FillAmount >= 1 && !neighbour.isFilling) lm.StartFillLoop(neighbour);
					}
				}
			}
		}
	}
}