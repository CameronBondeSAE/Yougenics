using System;
using System.Collections;
using System.Collections.Generic;
using Alex;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem.HID;
using Unity.Netcode;

class ShopSingleItem : MonoBehaviour, IInteractable
{
  [HideInInspector]
  public GameObject itemToSpawn;
  [HideInInspector]
  public Transform spawningPoint;
  public Spawner spawner;
  
  
  public ShopButton[] shopButtons;
  
  // Spawn point etc
  // Other stuff like price etc

  void Start()
  {
    foreach (ShopButton shopButton in shopButtons)
    {
      shopButton.buttonPressedEvent += SpawnItem;
    }
    
  }
  public void Interact()
  {
    
    spawner.SpawnSingle(itemToSpawn, spawningPoint.position, quaternion.identity);
    
  }

  public void SpawnItem(GameObject item, Transform spawnPoint)
  {
    itemToSpawn = item;
    spawningPoint = spawnPoint;
    Interact();
  }
  
  
}

