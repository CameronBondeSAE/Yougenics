using System;
using System.Collections;
using System.Collections.Generic;
using Alex;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class SingleItemShop : MonoBehaviour, IInteractable
{
    public Spawner spawner;
    public Transform spawningPoint;
    public IItem items;
    public Button button;

    private void Awake()
    {
        items = GetComponent(typeof(IItem)) as IItem;
        button.buttonPressedEvent += Interact;
    }

    public void Interact()
    {
        SpawnThis();
    }

    public void SpawnThis()
    {
        
        spawner.SpawnSingle(items.Item(), spawningPoint.position, Quaternion.identity);
        Interact();
    }
}





