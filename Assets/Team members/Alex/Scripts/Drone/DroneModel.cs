using System;
using System.Collections;
using System.Collections.Generic;
using NodeCanvas.Tasks.Actions;
using UnityEngine;
using UnityEngine.Serialization;

[SelectionBase]
public class DroneModel : MonoBehaviour, IFlyable, IInteractable
{
    public Rigidbody rb;
    public List<DroneWing> wings;
    public List<DroneWing> frontWings;
    public List<DroneWing> backWings;
    public float turningSpeed = 50f;
    public float movementSpeed = 20f;
    public bool isFlying;
    Energy energy;
    private float lockPos = 0;
    public Camera camera;

    void Awake()
    {
        energy = GetComponent<Energy>();
    }

    private void FixedUpdate()
    {
        CanFly();
    }

    public void AccelerateAndReverse(float amount)
    {
        if (isFlying)
        {
            foreach (DroneWing wing in wings)
            {
                wing.ApplyForwardForce(amount * movementSpeed);
                rb.transform.rotation = Quaternion.Euler(lockPos, lockPos, lockPos);
            }
        }
    }

    public void StrafeLeftAndRight(float amount)
    {
        if (isFlying)
        {
            foreach (DroneWing frontwing in frontWings)
            {
                frontwing.ApplySidewaysForce(amount * movementSpeed);
                frontwing.transform.localRotation = Quaternion.Euler(0, (turningSpeed * amount), 0);
            }

            foreach (DroneWing backwing in backWings)
            {
                backwing.ApplySidewaysForce(amount * movementSpeed);
                backwing.transform.localRotation = Quaternion.Euler(0, (turningSpeed * amount * -1f), 0);
            }
            
            //foreach (DroneWing wing in wings)
            {
                //wing.ApplySidewaysForce(amount * movementSpeed);
            }
        }
    }

    public void UpAndDown(float amount)
    {
        if (isFlying)
        {
            foreach (DroneWing wing in wings)
            {
                wing.ApplyUpwardForce(amount * movementSpeed * 5f);

            }
        }
    }

    private void CanFly()
    {
        //If drone has energy they will ignore gravity and fly, when they run out they will fall to the ground
        if (energy.EnergyAmount.Value > 0)
        {
            isFlying = true;
            rb.useGravity = false;
        }
        else
        {
            isFlying = false;
            rb.useGravity = true;
        }
    }


    public Vector3 GetExitPosition()
    {
        // TODO
        return Vector3.zero;
    }



    public void CameraUpAndDown(float amount)
    {
        camera.transform.localRotation = Quaternion.Euler(0, (turningSpeed * amount * -1f), 0);
        throw new System.NotImplementedException();
    }

    public void Interact()
    {
        throw new System.NotImplementedException();
    }

    public bool isItem()
    {
        return true;
    }
}