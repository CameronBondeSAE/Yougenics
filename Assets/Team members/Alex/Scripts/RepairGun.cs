using System;
using System.Collections;
using System.Collections.Generic;
using Minh;
using UnityEngine;

public class RepairGun : MonoBehaviour, IItem, IInteractable
{
    Health health;
    public ItemInfo    itemInfo;
    public Energy energy;
    public float energyPerShot = 10f;
    public bool canShoot;
    public float repairAmount = 10f;
    public float range = 10f;

    void Start()
    {
        canShoot = true;
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
        StartCoroutine(Shoot());
    }

    public IEnumerator Shoot()
    {
        if (energy.EnergyAmount.Value >= energyPerShot && canShoot) 
        {
            RaycastHit hitTarget;
            hitTarget = new RaycastHit();

            if (Physics.Raycast(transform.position, transform.forward, out hitTarget, range))
            {
                //Debug.DrawRay(transform.position, hitTarget.point, Color.red);
                health = hitTarget.collider.gameObject.GetComponentInParent<Health>();
                if (health != null)
                    health.ChangeHealth(repairAmount);

                Debug.Log("HIT: " + hitTarget.collider.gameObject.name);
                
            }
            else
            {
                Debug.Log("NothingHIT");
            }

            energy.ChangeEnergy(-energyPerShot);
            canShoot = false;
            yield return new WaitForSeconds(1f);
            canShoot = true;
        }
    }
    
}