using System;
using System.Collections;
using System.Collections.Generic;
using Anthill.Pool;
using UnityEngine;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine.InputSystem;
public class ItemSlot : MonoBehaviour
{
    
    public IItem slot1;
    public IItem slot2;
    public float interactDistance = 1f;
    public int adjust1 = 0;
    public Vector3 interactRayOffset = new Vector3(0, 0.5f, 0);

    public Transform Player;

    public Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        CheckWhatsInFrontOfMe();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rb.AddRelativeForce(0,100,0);
        RaycastHit hit = CheckWhatsInFrontOfMe();
        //if (hit.transform != null) Debug.Log(hit.transform.gameObject.name);
        if (hit.collider != null)
        {
            IItem item1 = hit.collider.gameObject.GetComponentInParent<IItem>();
            if (item1 != null)
            {
                slot1 = item1;
                //.transform.parent = object2.transform 
            }
            IItem item2 = hit.collider.gameObject.GetComponentInParent<IItem>();
            if (item2 != null)
            {
                slot2 = item2;
            }
        }
    }

    public void Update()
    {
        if (InputSystem.GetDevice<Mouse>().leftButton.wasPressedThisFrame)
        {
            IInteractable items1 = slot1 as IInteractable;
            items1.Interact();
            IItem item1 = slot1;
            slot1 = item1;
            //item1.pa == Player.parent.transform.position + new Vector3(0,Player.localPosition * adjust1 , 0);


        }
        if (InputSystem.GetDevice<Mouse>().rightButton.wasPressedThisFrame)
        {
            IInteractable items2 = slot2 as IInteractable;
            items2.Interact();
        } 
    }

    RaycastHit CheckWhatsInFrontOfMe()
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

    
}
