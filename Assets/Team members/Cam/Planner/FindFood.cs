using Anthill.AI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindFood : AntAIState
{
	public override void Enter()
	{
		base.Enter();
		
		// Do setup stuff ONCE
	}

	public override void Execute(float aDeltaTime, float aTimeScale)
	{
		base.Execute(aDeltaTime, aTimeScale);
		
		// Do stuff over time
	}

	public override void Exit()
	{
		base.Exit();
		
		// Clean up after myself
	}
}
