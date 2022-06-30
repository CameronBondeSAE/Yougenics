using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Cam
{
	public abstract class StateBase : MonoBehaviour
	{
		public StateManager stateManager;

		public virtual void Awake()
		{
			stateManager = GetComponent<StateManager>();
		}
	}
}
