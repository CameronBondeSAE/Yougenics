using Anthill.AI;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class StateViewInfo
{
	public string    info;
	public Texture2D icon;
	public AudioClip _audioClip;
}

// We're using ODIN hence the SerializedMonoBehaviour
public class Iconss : SerializedMonoBehaviour
{
	public Dictionary<Type, StateViewInfo> aiStateInfo;

	void Start()
	{
		// Manually defined dictionary (we're going to use ODIN though and do it in inspector)
		// aiStateInfo = new Dictionary<Type, StateViewInfo>();
		// aiStateInfo.Add(typeof(EatState), new StateViewInfo() { info = "Cam"});

		// Fake state we create to simulate AntAI creating states
		// FoundFoodState foundFoodState = gameObject.AddComponent<FoundFoodState>();
	}

	public void ChangeInfo(AntAIState aiState)
	{
		AudioClip audioClip                     = aiStateInfo[aiState.GetType()]._audioClip;
		if (audioClip != null)
		{
			//audioSource.clip = audioClip;
			//audioSource.Play();
		}

		Texture2D texture2D              = aiStateInfo[aiState.GetType()].icon;
		if (texture2D != null)
		{
			//hoverIcon = texture2D;	
		} 
	}
}