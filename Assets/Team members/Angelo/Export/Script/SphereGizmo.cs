using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereGizmo : MonoBehaviour
{
    
    
    void OnDrawGizmos()
    {
        Gizmos.DrawLine(new Vector3((transform.localScale.x/2),0,0) + transform.position, new Vector3(-transform.localScale.x/2, 0, 0) + transform.position);
        Gizmos.DrawLine(new Vector3(0, 0, transform.localScale.z/2) + transform.position, new Vector3(0, 0, -transform.localScale.z/2) + transform.position);
    }
}
