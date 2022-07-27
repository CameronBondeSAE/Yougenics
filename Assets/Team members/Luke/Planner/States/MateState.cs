using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Luke
{
	public class MateState : LukeAIState
	{
		public override void Enter()
		{
			base.Enter();
			critter.ChangeEmotion(Critter.Emotions.Mate);
		}

		public override void Execute(float aDeltaTime, float aTimeScale)
		{
			base.Execute(aDeltaTime, aTimeScale);
			critter.Mate();
		}
	}
}
