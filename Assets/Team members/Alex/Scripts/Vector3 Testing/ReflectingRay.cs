using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReflectingRay : MonoBehaviour
{
    public float RayCount = 5f;

    private void Update()
    {
        CastRay(transform.position, transform.forward);
    }

    private void CastRay(Vector3 position, Vector3 direction)
    {
        for (int i = 0; i < RayCount; i++)
        {
            Ray ray = new Ray(position, direction);
            RaycastHit hitInfo;

            if (Physics.Raycast(ray, out hitInfo))
            {
                Debug.DrawLine(position, hitInfo.point, Color.red);
                position = hitInfo.point;
                direction = Vector3.Reflect(direction,hitInfo.normal);
            }
            else
            {
                Debug.DrawRay(position, direction, Color.blue);
            }
        }
    }
}
