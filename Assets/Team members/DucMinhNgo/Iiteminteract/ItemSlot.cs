using System;
using System.Collections;
using System.Collections.Generic;
using Alex;
using Anthill.Pool;
using John;
using UnityEngine;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine.InputSystem;

namespace Minh
{
    public class ItemSlot : MonoBehaviour
    {
        public IItem slot1;
        public IItem slot2;
        public float interactDistance = 1f;
        public Vector3 interactRayOffset = new Vector3(0, 0.5f, 0);
        public float adjust1;
        public float adjust2;
        public float adjust3;
        public PlayerModel playerModel;
        public EnergyBarAlex energyBarItem1UI;
        public EnergyBarAlex energyBarItem2UI;
        public ItemNameUI itemName1UI;
        public ItemNameUI itemName2UI;
        public event Action<bool> item1PickedUpEvent;
        public event Action<bool> item2PickedUpEvent;


        public void Update()
        {
            RaycastHit hit;
            //if (hit.transform != null) Debug.Log(hit.transform.gameObject.name);
            if (InputSystem.GetDevice<Keyboard>().digit1Key.wasPressedThisFrame)
            {
                hit = playerModel.CheckWhatsInFrontOfMe();
                if(slot1 != null)
                {
                    MonoBehaviour monoBehaviour = slot1 as MonoBehaviour;
                    slot1 = null;
                    monoBehaviour.transform.parent = null;
                    monoBehaviour.GetComponent<Collider>().enabled = true;
                    
                    item1PickedUpEvent?.Invoke(false);
                    Debug.Log("dropped");
                    hit.collider.enabled = true;

                }
                else if (hit.collider != null)
                {
                    IItem item1 = hit.collider.gameObject.GetComponentInParent<IItem>();
                    if (item1 != null)
                    {
                        slot1 = item1;
                        MonoBehaviour monoBehaviour = item1 as MonoBehaviour;
                        monoBehaviour.transform.parent   = playerModel.transform;
                        monoBehaviour.transform.position = playerModel.transform.position + new Vector3(0f, adjust2 * adjust3 * Time.deltaTime, 1 * adjust1 * Time.deltaTime);
                        monoBehaviour.transform.rotation = playerModel.transform.rotation;
                        //Rigidbody rb = item1 as Rigidbody;

                        hit.collider.enabled = false;
                        
                        if (monoBehaviour.GetComponent<Rigidbody>())
                        {
                            monoBehaviour.GetComponent<Rigidbody>().isKinematic = true;
                            monoBehaviour.GetComponent<Collider>().enabled = false;
                        }

                        itemName1UI.name = slot1.GetInfo().name;
                        
                        item1PickedUpEvent?.Invoke(true);
                        energyBarItem1UI.myEnergy = monoBehaviour.GetComponent<Energy>();
                        
                        Debug.Log("picked up");
                    }
                }
            }
            if (InputSystem.GetDevice<Keyboard>().digit2Key.wasPressedThisFrame)
            {
                hit = playerModel.CheckWhatsInFrontOfMe();
                if(slot2 != null)
                {
                    MonoBehaviour monoBehaviour = slot2 as MonoBehaviour;
                    slot2 = null;
                    monoBehaviour.transform.parent = null;
                    monoBehaviour.GetComponent<Collider>().enabled = true;
                    
                    item2PickedUpEvent?.Invoke(false);
                    
                    //energyBarItem2UI.isVisible = false;
                    Debug.Log("dropped");
                    hit.collider.enabled = true;
                }
                else if (hit.collider != null)
                {
                    IItem item2 = hit.collider.gameObject.GetComponentInParent<IItem>();
                    if (item2 != null)
                    {
                        slot2 = item2;
                        MonoBehaviour monoBehaviour = item2 as MonoBehaviour;
                        monoBehaviour.transform.parent   = playerModel.transform;
                        monoBehaviour.transform.position = playerModel.transform.position + new Vector3(0f, adjust2 * adjust3 * Time.deltaTime, 1 * adjust1 * Time.deltaTime);
                        monoBehaviour.transform.rotation = playerModel.transform.rotation;
                        if (monoBehaviour.GetComponent<Rigidbody>())
                        {
                            monoBehaviour.GetComponent<Rigidbody>().isKinematic = true;
                            monoBehaviour.GetComponent<Collider>().enabled = false;
                        }

                        hit.collider.enabled = false;


                        item2PickedUpEvent?.Invoke(true);
                        itemName2UI.name = slot2.GetInfo().name;
                        energyBarItem2UI.myEnergy = monoBehaviour.GetComponent<Energy>();
                        Debug.Log("picked up");
                    }
                }
            }
            if (InputSystem.GetDevice<Mouse>().leftButton.wasPressedThisFrame)
            {
                IInteractable items1 = slot1 as IInteractable;
                if (items1 != null) items1.Interact();
            }

            if (InputSystem.GetDevice<Mouse>().rightButton.wasPressedThisFrame)
            {
                IInteractable items2 = slot2 as IInteractable;
                if (items2 != null) items2.Interact();
            }
        }
    }
}




