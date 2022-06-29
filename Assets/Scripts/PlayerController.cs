using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject vehicleToControl;

    // Update is called once per frame
    void FixedUpdate()
    {
        // Get the interface. Don't about the rest of the GameObject AT ALL.
        // HACK. Normally you'd assign this directly to the component, but SHOCK Unity doesn't support Interfaces in the inspector so it's annoying to test
        if (vehicleToControl != null)
        {
            IVehicleControls vehicleControls = vehicleToControl.GetComponent<IVehicleControls>();

            if (vehicleControls != null)
            {
                vehicleControls.Steer(Input.GetAxis("Horizontal"));
                vehicleControls.AccelerateAndReverse(Input.GetAxis("Vertical"));
            }
        }
    }
}
