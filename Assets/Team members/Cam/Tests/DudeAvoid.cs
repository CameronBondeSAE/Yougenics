using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DudeAvoid : MonoBehaviour
{
    Rigidbody    rb;
    public float turnForce = 1000f;
    [SerializeField]
    float moveForce = 1000f;

    [SerializeField]
    float distance = 5f;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // transform.forward

        // Quaternion rotatedQ = transform.rotation * Quaternion.Euler(0, 30f, 0);

        // Quaternion rotatedQ = Quaternion.AngleAxis(30, Vector3.up);
        
        // Quaternion.LookRotation()

        int count = 25;
        for (int i = -count; i < count; i++)
        {
            for (int p = -count; p < count; p++)
            {
                Vector3 dir = Quaternion.Euler(p, i, 0) * Vector3.forward;

                dir = transform.TransformDirection(dir);

                Debug.DrawRay(transform.position, dir * 10f, Color.green);
            }
        }
        // Debug.DrawRay(transform.position, transform.forward, Color.green);


        RaycastHit hitInfo;
        if (Physics.Raycast(new Ray(transform.position, transform.forward), out hitInfo, distance, 255, QueryTriggerInteraction.Ignore))
        {
            rb.AddRelativeTorque(0, turnForce, 0, ForceMode.Impulse);
        }
        else
        {
            // HACK: Move to separate steering behaviour component, but here at least it's easy to alter speed based on collisions
            rb.AddRelativeForce(0, 0, moveForce, ForceMode.Impulse);
        }
        
    }
}
