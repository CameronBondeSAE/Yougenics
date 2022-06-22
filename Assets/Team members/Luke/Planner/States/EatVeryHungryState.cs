using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Luke
{
	public class EatVeryHungryState : LukeAIState
	{
		public override void Enter()
		{
			base.Enter();
			critter.ChangeEmote(0);
		}
		
		public override void Execute(float aDeltaTime, float aTimeScale)
		{
			base.Execute(aDeltaTime, aTimeScale);
			critter.Eat();
		}
	}
}
