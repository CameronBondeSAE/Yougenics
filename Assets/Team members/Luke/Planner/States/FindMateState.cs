using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Luke
{
	public class FindMateState : LukeAIState
	{
		public override void Enter()
		{
			base.Enter();
			critter.ChangeEmotion(Critter.Emotions.Wander);
		}

		public override void Execute(float aDeltaTime, float aTimeScale)
		{
			base.Execute(aDeltaTime, aTimeScale);
			critter.IterateBiomes();
			critter.MoveBiomes();
		}

		public override void Exit()
		{
			base.Exit();
		}
	}
}
