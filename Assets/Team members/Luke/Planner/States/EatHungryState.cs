using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Luke
{
	public class EatHungryState : LukeAIState
	{
		public override void Enter()
		{
			base.Enter();
			critter.ChangeEmotion(Critter.Emotions.Hungry);
		}

		public override void Execute(float aDeltaTime, float aTimeScale)
		{
			base.Execute(aDeltaTime, aTimeScale);
			critter.Eat();
		}

		public override void Exit()
		{
			base.Exit();
		}
	}
}
