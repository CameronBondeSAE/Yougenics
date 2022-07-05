using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerModel : MonoBehaviour
{
    [Header("Player Setup")]
    public float movementSpeed = 10f;
    public float lookSensitivity = 50f;
    public float jumpHeight = 5f;
    public float interactDistance = 1f;
    public Vector3 interactRayOffset = new Vector3(0, 0.5f, 0);
    public Rigidbody rb;

    //Input control variables
    Vector3 movement;
    [HideInInspector]
    public float mouseX;

    private void Start()
    {
        GetComponent<John.PlayerController>().OnPlayerAssigned();
    }

    private void Update()
    {
        transform.Rotate(Vector3.up * mouseX);
    }
    private void FixedUpdate()
    {
        //Using forces & mass for movement
        //rb.AddForce(movement * speed, ForceMode2D.Force);

        //instant reactive movement
        rb.MovePosition(rb.position + movement * movementSpeed * Time.fixedDeltaTime);
    }

    #region Input Controlls

    public void Movement(Vector2 movementInput)
    {
        movement = new Vector3(movementInput.x, 0, movementInput.y);
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
        }
        else
        {
            Debug.Log("Nothing found");
        }
    }

    public void Jump()
    {
        if (rb.velocity.y > 0)
            return;

        rb.AddForce(0, jumpHeight, 0, ForceMode.VelocityChange);
    }

    #endregion

    RaycastHit CheckWhatsInFrontOfMe()
    {
        // Check what's in front of me. TODO: Make it scan the area or something less precise
        RaycastHit hit;
        // Ray        ray = new Ray(transform.position + transform.TransformPoint(interactRayOffset), transform.forward);
        // NOTE: TransformPoint I THINK includes the main position, so you don't have to add world position to the final
        Vector3 transformPoint = transform.TransformPoint(interactRayOffset);
        // Debug.Log(transformPoint);
        Ray ray = new Ray(transformPoint, transform.forward);

        Debug.DrawRay(ray.origin, ray.direction * interactDistance, Color.green, 2f);

        // if (Physics.Raycast(ray, out hit, interactDistance))
        Physics.SphereCast(ray, 0.5f, out hit, interactDistance);

        return hit;
    }
}
