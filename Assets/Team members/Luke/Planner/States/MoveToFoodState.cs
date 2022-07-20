using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Luke
{
	public class MoveToFoodState : LukeAIState
	{
		public override void Enter()
		{
			base.Enter();
			critter.ChangeEmotion(0);
		}

		public override void Execute(float aDeltaTime, float aTimeScale)
		{
			base.Execute(aDeltaTime, aTimeScale);
			critter.LocateNearestFood();
			critter.MoveToNearestFood();
		}
	}
}