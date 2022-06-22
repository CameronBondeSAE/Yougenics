using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Luke
{
	public class DefaultBehaviourHungryState : LukeAIState
	{
		public override void Enter()
		{
			base.Enter();
			critter.WakeUp();
		}

		public override void Execute(float aDeltaTime, float aTimeScale)
		{
			base.Execute(aDeltaTime, aTimeScale);
			if (critter.IsFoodListEmpty())
			{
				Wander();
				return;
			}
			critter.ChangeEmote(0);
			if (!critter.LocateNearestFood())
			{
				Wander();
				return;
			}
			critter.MoveToNearestFood();
			if (!critter.CheckHasFood()) return;
			critter.Eat();
		}

		private void Wander()
		{
			critter.ChangeEmote(4);
			critter.IterateBiomes();
			critter.MoveBiomes();
		}
	}
}
