using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace Cam
{
	public class ButtonViewModel : MonoBehaviour
	{
		public Cam.Button button;

		public Transform      buttonMesh;
		public ParticleSystem sparks;
		public AudioSource    sound;

		public Vector3 buttonStartPosition;
		public Transform buttonInsetPosition;
		
		// Start is called before the first frame update
		void Start()
		{
			buttonStartPosition = buttonMesh.position;
			
			button.PressedEvent += ButtonOnPressedEvent;
			button.FinishedEvent += ButtonOnFinishedEvent;
		}

		void ButtonOnFinishedEvent()
		{
			Sequence seq = DOTween.Sequence();
			seq.Append(buttonMesh.DOMove(buttonStartPosition, 0.5f));
			seq.Play();
			
			sound.Stop();
		}

		[ServerRpc(RequireOwnership = false)]
		void ButtonOnPressedEvent()
		{
			Sequence seq = DOTween.Sequence();
			seq.Append(buttonMesh.DOPunchScale(new Vector3(2f, 2f, 2f), 0.25f));
			seq.Append(buttonMesh.DOLocalMove(buttonInsetPosition.localPosition, 1f, false));
			seq.Play();
			sparks.Emit(20);
			sound.Play();
		}
	}
}