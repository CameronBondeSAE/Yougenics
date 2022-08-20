using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Luke
{
	public class WakeUpImminentDangerState : LukeAIState
	{
		public override void Execute(float aDeltaTime, float aTimeScale)
		{
			base.Execute(aDeltaTime, aTimeScale);
			critter.WakeUp();
		}
	}
}
