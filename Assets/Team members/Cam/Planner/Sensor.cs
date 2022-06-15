using Anthill.AI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cam
{
	public class Sensor : MonoBehaviour, ISense
	{
		public enum ManWithAPlanConditions
		{
			isHealthy    = 0,
			hasFood      = 1,
			canSeeTarget = 2
		}
		
		public Vision vision;
		
		public void CollectConditions(AntAIAgent aAgent, AntAICondition aWorldState)
		{
			CamsDude camsDude = GetComponent<CamsDude>();
			
			// Set ALL conditions, yes, even the ones you don't use
			aWorldState.Set(ManWithAPlanConditions.isHealthy, camsDude.isHealthy);
			aWorldState.Set(ManWithAPlanConditions.hasFood, camsDude.hasFood);
			aWorldState.Set(ManWithAPlanConditions.canSeeTarget, vision.RefreshVision());
		}
	}
}