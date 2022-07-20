using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Luke
{
	public class GoToSleepVeryTiredState : LukeAIState
	{
		public override void Enter()
		{
			base.Enter();
			critter.ChangeEmotion(Critter.Emotions.Tired);
		}

		public override void Execute(float aDeltaTime, float aTimeScale)
		{
			base.Execute(aDeltaTime, aTimeScale);
			critter.GoToSleep();
		}
	}
}
