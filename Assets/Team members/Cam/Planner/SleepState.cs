using Anthill.AI;
using Cam;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SleepState : AntAIState
{

	public override void Create(GameObject aGameObject)
	{
		base.Create(aGameObject);

	}

	public override void Enter()
	{
		base.Enter();

		Debug.Log("Entered Sleep");
	}
	
}
