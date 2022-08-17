using System;
using System.Collections;
using System.Collections.Generic;
using Minh;
using UnityEngine;

public class Gun : MonoBehaviour, IItem, IInteractable
{
    Health health;
    public ItemInfo    itemInfo;
    Energy energy;
    public float energyPerShot = 20f;

    void Start()
    {
        energy = GetComponent<Energy>();
        energy.EnergyAmount.Value = energy.energyMax;
    }
    public void SpawnedAsNormal()
    {
        throw new System.NotImplementedException();
    }

    public ItemInfo GetInfo()
    {
        return itemInfo;
    }

    public void Interact()
    {
        Shoot();
    }

    public void Shoot()
    {
        if (energy.EnergyAmount.Value >= 1)
        {
            RaycastHit hitTarget;
            hitTarget = new RaycastHit();


            if (Physics.Raycast(transform.position, transform.forward, out hitTarget, 50))
            {
                //Debug.DrawRay(transform.position, transform.forward, Color.red);
                health = hitTarget.collider.gameObject.GetComponentInParent<Health>();
                if (health != null)
                health.ChangeHealth(-20);

                Debug.Log("HIT: " + hitTarget.collider.gameObject.name);
            }
            else
            {
                Debug.Log("NothingHIT");
            }

            energy.ChangeEnergy(-energyPerShot);
        }
    }
}
