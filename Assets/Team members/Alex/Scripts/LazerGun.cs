using System;
using System.Collections;
using System.Collections.Generic;
using Minh;
using UnityEngine;

public class LazerGun : MonoBehaviour, IItem, IInteractable
{
    Health health;
    public ItemInfo    itemInfo;
    public Energy energy;
    public float energyPerShot = 20f;
    public bool canShoot;
    public float lazerDamage = 20f;

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

            if (Physics.Raycast(transform.position, transform.forward, out hitTarget, 50))
            {
                //Debug.DrawRay(transform.position, hitTarget.point, Color.red);
                health = hitTarget.collider.gameObject.GetComponentInParent<Health>();
                if (health != null)
                health.ChangeHealth(lazerDamage);

                Debug.Log("HIT: " + hitTarget.collider.gameObject.name);
                
            }
            else
            {
                Debug.Log("NothingHIT");
            }

            energy.ChangeEnergy(-energyPerShot);
            canShoot = false;
            yield return new WaitForSeconds(2f);
            canShoot = true;
        }
    }
    
}
