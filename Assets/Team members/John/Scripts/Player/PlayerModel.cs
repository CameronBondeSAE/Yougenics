using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.InputSystem;

public class PlayerModel : NetworkBehaviour
{
    [Header("Player Setup")]
    public float movementSpeed = 10f;
    public float lookSensitivity = 50f;
    public float jumpHeight = 5f;
    public float interactDistance = 1f;
    public Vector3 interactRayOffset = new Vector3(0, 0.5f, 0);
    public Rigidbody rb;
    public GameObject myCam;

    //Input control variables
    Vector3 movement;
    [HideInInspector]
    public float mouseX, mouseY;

    [Header("For Non Networking Setup")]
    public John.PlayerController controller;
    public PlayerInput playerInput;

    public Transform playerHead;

    public override void OnNetworkSpawn()
    {
        if (!IsOwner)
            myCam.SetActive(false);
    }

    //Setting up controller if non-networked
    private void Start()
    {
        if(NetworkManager.Singleton == null)
        {
            Debug.Log("No Network Manager found - Using local controller");
            controller.enabled = true;
            playerInput.enabled = true;
            controller.OnPlayerAssignedNonNetworked(this);
        }
    }

    private void Update()
    {
        transform.Rotate(new Vector3(0, mouseX * Time.deltaTime * lookSensitivity,0), Space.Self);
    }
    private void FixedUpdate()
    {
        //Using forces & mass for movement
        rb.AddForce(movement * movementSpeed, ForceMode.Impulse);

        //instant reactive movement
        //rb.MovePosition(rb.position + movement * movementSpeed * Time.fixedDeltaTime);
    }

    #region Input Controlls Networked

    [ClientRpc]
    public void MovementClientRpc(Vector2 movementInput)
    {
        movement = transform.right * movementInput.x + transform.forward * movementInput.y;
    }

    [ClientRpc]
    public void MouseXClientRpc(float value)
    {
        mouseX = value;
    }

    [ClientRpc]
    public void MouseYClientRpc(float value)
    {
        mouseY = value;
    }

    [ClientRpc]
    public void InteractClientRpc()
    {
        RaycastHit hit = CheckWhatsInFrontOfMe();

        if (hit.collider != null)
        {
            // Get in parent, because the collider might be a child and IInteractable should be on the root GO
            IInteractable interactable = hit.collider.gameObject.GetComponentInParent<IInteractable>();
            if (interactable != null)
            {
                interactable.Interact();
            }
            else
                Debug.Log("Collider Not Interactable");
        }
        else
        {
            Debug.Log("Nothing found");
        }
    }

    [ClientRpc]
    public void JumpClientRpc()
    {
        //TODO: Add jump restriction when already in the air

        rb.AddForce(0, jumpHeight, 0, ForceMode.VelocityChange);
    }

    #endregion

    #region Input Controlls Non-Networked
    public void Movement(Vector2 movementInput)
    {
        movement = transform.right * movementInput.x + transform.forward * movementInput.y;
    }

    public void MouseX(float value)
    {
        mouseX = value;
    }

    public void MouseY(float value)
    {
        mouseY = value;
    }

    public void Interact()
    {
        RaycastHit hit = CheckWhatsInFrontOfMe();

        if (hit.collider != null)
        {
            IInteractable interactable = hit.collider.gameObject.GetComponentInParent<IInteractable>();
            if (interactable != null)
            {
                interactable.Interact();
            }
            else
                Debug.Log("Collider Not Interactable");
        }
        else
        {
            Debug.Log("Nothing found");
        }
    }

    public void Jump()
    {
        //TODO: Add jump restriction when already in the air

        rb.AddForce(0, jumpHeight, 0, ForceMode.VelocityChange);
    }
    #endregion

    RaycastHit CheckWhatsInFrontOfMe()
    {
        // Check what's in front of me. TODO: Make it scan the area or something less precise
        RaycastHit hit;
        // Ray        ray = new Ray(transform.position + transform.TransformPoint(interactRayOffset), transform.forward);
        // NOTE: TransformPoint I THINK includes the main position, so you don't have to add world position to the final
        Vector3 transformPoint = playerHead.TransformPoint(interactRayOffset);
        // Debug.Log(transformPoint);
        Ray ray = new Ray(transformPoint, playerHead.forward);

        Debug.DrawRay(ray.origin, ray.direction * interactDistance, Color.green, 2f);

        // if (Physics.Raycast(ray, out hit, interactDistance))
        Physics.SphereCast(ray, 0.5f, out hit, interactDistance);

        return hit;
    }
}
