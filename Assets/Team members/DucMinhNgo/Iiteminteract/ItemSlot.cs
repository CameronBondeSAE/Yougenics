using System;
using System.Collections;
using System.Collections.Generic;
using Alex;
using Anthill.Pool;
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
        public Transform Player;

        // Update is called once per frame
        public void FixedUpdate() 
        {
            RaycastHit hit = CheckWhatsInFrontOfMe();
            //if (hit.transform != null) Debug.Log(hit.transform.gameObject.name);
            if (InputSystem.GetDevice<Keyboard>().digit1Key.wasPressedThisFrame)
            {
                if(slot1 != null)
                {
                    MonoBehaviour monoBehaviour = slot1 as MonoBehaviour;
                    slot1 = null;
                    monoBehaviour.transform.parent = null;
                    Debug.Log("dropped");
                } 
                if (hit.collider != null)
                {
                    IItem item1 = hit.collider.gameObject.GetComponentInParent<IItem>();
                    if (item1 != null)
                    {
                        slot1 = item1;
                        MonoBehaviour monoBehaviour = item1 as MonoBehaviour;
                        monoBehaviour.transform.parent = Player.transform;
                        monoBehaviour.transform.position = Player.transform.position + new Vector3(0f, adjust2 * adjust3 * Time.deltaTime, 1 * adjust1 * Time.deltaTime);
                        Debug.Log("picked up");
                    }
                }
                
            }
        }
        private RaycastHit CheckWhatsInFrontOfMe()
        {
            // Check what's in front of me. TODO: Make it scan the area or something less precise
            RaycastHit hit;
            // Ray        ray = new Ray(transform.position + transform.TransformPoint(interactRayOffset), transform.forward);
            // NOTE: TransformPoint I THINK includes the main position, so you don't have to add world position to the final
            Vector3 transformPoint = Player.TransformPoint(interactRayOffset);
            // Debug.Log(transformPoint);
            Ray ray = new Ray(transformPoint, Player.forward);

            Debug.DrawRay(ray.origin, ray.direction * interactDistance, Color.green, 2f);

            // if (Physics.Raycast(ray, out hit, interactDistance))
            Physics.SphereCast(ray, 0.5f, out hit, interactDistance);
            return hit;
        }
        public void Update()
        {
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




