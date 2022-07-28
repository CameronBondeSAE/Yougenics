using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Anthill.Effects;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class Vector3Testing : MonoBehaviour
{
    public Vector3 test;

    private void Update()
    {
        Reflect();
        //Debug.DrawLine(new Vector3(0,0,0), test.normalized, Color.red);
        
    

        //Adds the power of 2 of each axis together
        //Debug.Log("sqrMagnitude = "+test.sqrMagnitude);
        
        //Axis values can never be greater than 1 or less than -1
        //Line being drawn always has length of 1 
        //0,0,0 values draws no line 
        //Debug.Log("normalized = "+test.normalized);
        
        
        //Uses the full positive value of the Y axis
        //So it's getting a height value of something more than a length/width 
        //Debug.Log("magnitude = "+test.magnitude);

        //Draws a line up
        //Always positive number 
        //???
        //Debug.Log(Vector3.Angle(Vector3.forward, test));
       

        
        //Debug.Log(Vector3.Cross(Vector3.up, test));
        //Debug.DrawLine(new Vector3(0,0,0), Vector3.Cross(Vector3.up, test), Color.red);
        
        //Debug.Log(Vector3.Dot(Vector3.up, test));
        //Debug.DrawLine(0, Vector3.Dot(Vector3.up, test));
        
        //Debug.Log(Vector3.Reflect(Vector3.up, test));
        //Debug.Log(Vector3.ProjectOnPlane(Vector3.up, test));
        //Debug.Log(Vector3.Project(Vector3.up, test));
    }

    private void Reflect()
    {
        //Debug.DrawLine(new Vector3(0, 0, 0), Vector3.Reflect(Vector3.up, test));
        float maxdistance = 255f;
        
        
        

        for (int i = 0; i < 5; i++)
        {
            Ray ray = new Ray(transform.position, transform.forward);
            RaycastHit hitInfo;
            Physics.Raycast(ray, out hitInfo);

            Vector3 reflect = Vector3.Reflect(ray.direction, hitInfo.normal);
            Debug.DrawLine(ray.origin, hitInfo.point, Color.red);
        }
        
        /*
        Ray reflectedRayDirection = new Ray(hitInfo.point, reflect);
        RaycastHit reflectedhitInfo;
        
        Physics.Raycast(reflectedRayDirection, out reflectedhitInfo);
        Debug.DrawLine(hitInfo.point, reflectedhitInfo.point, Color.blue);
        Vector3 reflectedRay = Vector3.Reflect(reflectedRayDirection.direction, reflectedhitInfo.normal);
        

        //raycast = Physics.Raycast(ray, out hitInfo, maxDistance);
        //reflectedDirection  = Vector3.Reflect(direction, hitInfo.normal);
        */
    }
}
