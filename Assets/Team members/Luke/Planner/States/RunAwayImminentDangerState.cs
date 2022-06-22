using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Luke
{
	public class RunAwayImminentDangerState : LukeAIState
	{
		public override void Enter()
		{
			base.Enter();
			critter.ChangeEmote(2);
		}

		public override void Execute(float aDeltaTime, float aTimeScale)
		{
			base.Execute(aDeltaTime, aTimeScale);
			critter.LocateNearestPredator();
			critter.MoveAwayFromPredator();
		}
	}
}
