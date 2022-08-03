using System;
using System.Collections;
using System.Collections.Generic;
using Minh;
using UnityEngine;

public class Gun : MonoBehaviour, IItem, IInteractable
{
    Health health;
    public ItemInfo    itemInfo;

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
        RaycastHit hitTarget;
        hitTarget = new RaycastHit();

        if (Physics.Raycast(transform.position, transform.forward, out hitTarget, 255))
        {
            health = hitTarget.collider.gameObject.GetComponent<Health>();
            health.CurrentHealth.Value -= 20;
            Debug.Log("HIT");
        }
        else
        {
            Debug.Log("NothingHIT");
        }
    }
}
