using John;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.InputSystem;

namespace John
{
	public class PlayerModel : NetworkBehaviour
	{
		[Header("Player Setup")]
		public float movementSpeed = 10f;
		float defaultMovementSpeed;
		public float sprintSpeed = 15f;

		public float   lookSensitivity   = 50f;
		public float   jumpHeight        = 5f;
		public float   interactDistance  = 1f;
		public Vector3 interactRayOffset = new Vector3(0, 0.5f, 0);

		bool onGround = false;
		public float      onGroundDrag = 4f;
		public float      inAirDrag    = 0.1f;
		public Rigidbody  rb;
		
		public Transform  cameraMount;

		//Input control variables
		public Vector3 movement;

		[HideInInspector]
		public float mouseX, mouseY;

		[HideInInspector]
		public ClientInfo myClientInfo;
		/*public event Action onClientAssignedEvent;
	
		public void OnClientAssigned(ClientInfo client)
		{
			myClientInfo = client;
			onClientAssignedEvent?.Invoke();
		}*/

		public Transform playerHead;

		public bool      inVehicle = false;
		public IVehicleControls vehicleReference;

		//EVENTS
		public event Action onJumpEvent;
		public event Action onDeathEvent;
		public event Action<Vector2> onMovementEvent;
		public event Action<float> onMovementSpeedEvent;

		//Setting up controller if non-networked
		private void Start()
		{
			if (NetworkManager.Singleton == null)
			{
				Debug.Log("No Network Manager found - Load level using ManagerScene or Drag ManagerScene into your Level");
			}

			defaultMovementSpeed = movementSpeed;
			GetComponent<Minh.Health>().DeathEvent += PlayerDeath;
		}

        private void PlayerDeath()
        {
			//What should happen?

			//View Stuff
			onDeathEvent?.Invoke();
        }

        private void Update()
		{
			transform.Rotate(new Vector3(0, mouseX * Time.deltaTime * lookSensitivity, 0), Space.Self);
		}

		private void FixedUpdate()
		{
			if (onGround)
			{
				//Using forces & mass for movement
				rb.AddRelativeForce(movement * movementSpeed, ForceMode.Impulse);
			}

			//instant reactive movement
			//rb.MovePosition(rb.position + movement * movementSpeed * Time.fixedDeltaTime);

			// Ground check for jumping
			// Hack: Ray length
			if (Physics.Raycast(new Ray(transform.position, Vector3.down), 1.1f))
			{
				onGround = true;
				rb.drag  = onGroundDrag;
			}
			else
			{
				onGround = false;
				rb.drag  = inAirDrag;
			}
		}

		#region Client Side Functionality

		[ClientRpc]
		public void MovementClientRpc(Vector2 movementInput)
		{
			// View Stuff

			onMovementEvent?.Invoke(movementInput);

			//HACK: This is only called when movement is first pressed or released, so using a vector2.zero check to see if we are moving or not- speed is used to set running animations
			if (movementInput != Vector2.zero)
				onMovementSpeedEvent?.Invoke(1);
			else
				onMovementSpeedEvent?.Invoke(0);
		}

		[ClientRpc]
		public void MouseXClientRpc(float value)
		{

			//View Stuff
		}

		[ClientRpc]
		public void MouseYClientRpc(float value)
		{
			//View Stuff
		}

		[ClientRpc]
		public void InteractClientRpc()
		{
			// View stuff
		}


		[ClientRpc]
		public void JumpClientRpc()
		{
			//View Stuff
			onJumpEvent?.Invoke();
		}

		[ClientRpc]
		public void SprintClientRpc(bool isSprinting)
		{
			//View Stuff
		}

		#endregion

		#region Server Side Functionality

		public void Movement(Vector2 movementInput)
		{
			if (inVehicle)
			{
				vehicleReference.AccelerateAndReverse(movementInput.y);
				vehicleReference.Steer(movementInput.x);
			}
			else
			{
				// movement = transform.right * movementInput.x + transform.forward * movementInput.y;
				movement.x = movementInput.x;
				movement.z = movementInput.y;
			}
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
			// Get OUT of vehicle
			if (inVehicle)
			{
				GetOutOfVehicle();
				return;
			}
			
			
			// Normal interact with items in the scene
			RaycastHit hit = CheckWhatsInFrontOfMe();

			if (hit.collider != null)
			{
				// HACK: Bit hard coded
				vehicleReference = hit.collider.gameObject.GetComponentInParent<IVehicleControls>();
				if (vehicleReference != null)
				{
					GetInVehicle();
				}

				
				
				IInteractable interactable = hit.collider.gameObject.GetComponentInParent<IInteractable>();
				if (interactable != null)
				{
					interactable.Interact();
				}
				// else
					// Debug.Log("Collider Not Interactable");
			}
			// else
			// {
				// Debug.Log("Nothing found");
			// }
		}

		public void Jump()
		{
			if (onGround)
			{
				rb.AddForce(0, jumpHeight, 0, ForceMode.VelocityChange);
			}
		}

		public void Sprint(bool isSprinting)
        {
			if(isSprinting)
            {
				movementSpeed = sprintSpeed;
            }
			else
            {
				movementSpeed = defaultMovementSpeed;
            }
        }

		#endregion
		
		public void GetInVehicle()
		{
			// TODO: This only works if there's one collider on root
			inVehicle                        = true;
			GetComponent<Collider>().enabled = false;
			rb.isKinematic                   = true;
			// GetInVehicleEvent?.Invoke(true);


			// Lock me to the vehicle, just so the camera doesn't need to retarget anything. I don't actually need to be a child
			MonoBehaviour vehicleComponent = vehicleReference as MonoBehaviour;
			transform.parent        = vehicleComponent.transform;
			transform.localPosition = Vector3.zero;

			// vehicleReference.Enter();
		}

		public void GetOutOfVehicle()
		{
			// TODO: This only works if there's one collider on root
			inVehicle                        = false;
			GetComponent<Collider>().enabled = true;
			rb.isKinematic                   = false;
			// GetInVehicleEvent?.Invoke(false);

			// UNLock me from the vehicle, just so the camera doesn't need to retarget anything. I don't actually need to be a child
			transform.parent = null;

			// Put player at exit point on vehicle
			transform.position = vehicleReference.GetExitPosition();
			// transform.rotation = vehicleReference.GetVehicleExitPoint().rotation;

			transform.rotation = Quaternion.identity;
			
			// vehicleReference.Exit();
		}


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
}