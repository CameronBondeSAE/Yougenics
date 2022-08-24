using System;
using System.Collections;
using System.Collections.Generic;
using Tanks;
using UnityEngine;
using Object = UnityEngine.Object;

public class VehicleModel : MonoBehaviour, IVehicleControls, IItem, IInteractable
{
	public ItemInfo    itemInfo;
	public List<Wheel> wheels;
	public List<Wheel> frontWheels;
	public float       turningSpeed  = 50f;
	public float       movementSpeed = 20f;
	public Transform   outPos;
	public Transform   playerMount;
	public float       annoyingValue;


	public void AccelerateAndReverse(float amount)
	{
		annoyingValue = amount;
	}

	private void FixedUpdate()
	{
		//Moves each wheel attached to vehicle forward and backwards
		foreach (Wheel wheel in wheels)
		{
			wheel.ApplyForwardForce(annoyingValue * movementSpeed);
		}
	}

	public void Steer(float amount)
	{
		//Rotates only the front two wheels to allow vehicle to steer  
		foreach (Wheel frontWheel in frontWheels)
		{
			frontWheel.transform.localRotation = Quaternion.Euler(0, (turningSpeed * amount), 0);
		}
	}

	public Vector3 GetExitPosition()
	{
		return outPos.transform.position;
	}

	public Transform GetPlayerMountPosition()
	{
		return playerMount.transform;
	}


	public void SpawnedAsNormal()
	{
		throw new NotImplementedException();
	}

	public ItemInfo GetInfo()
	{
		return itemInfo;
	}

	public object transform { get; set; }
	public void   Interact()
	{
		
	}
}