using Cam;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DoorReactToButton : MonoBehaviour
{
	public Alex.Button    button;
	public Alex.Button    button2;
	// public DoorModel door;

	public UnityEvent activateOnAllButtonsPressed;

	bool buttonWasPressed;
	bool button2WasPressed;
	
	void Awake()
	{
		button.buttonPressedEvent += ButtonOnPressedEvent;
		button2.buttonPressedEvent += ButtonOnPressedEvent2;
	}

	void ButtonOnPressedEvent2()
	{
		button2WasPressed = true;
		CheckAllButtons();
	}

	void ButtonOnPressedEvent()
	{
		buttonWasPressed = true;
		CheckAllButtons();
	}

	void CheckAllButtons()
	{
		if (buttonWasPressed && button2WasPressed)
		{
			activateOnAllButtonsPressed?.Invoke();
		}
	}
}