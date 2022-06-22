using System.Collections;
using System.Collections.Generic;
using Anthill.AI;
using UnityEngine;

namespace Luke
{
	public class LukeAIState : AntAIState
	{
		protected Critter critter;
		
		public override void Create(GameObject aGameObject)
		{
			base.Create(aGameObject);

			critter = aGameObject.GetComponent<Critter>();
		}
	}
}