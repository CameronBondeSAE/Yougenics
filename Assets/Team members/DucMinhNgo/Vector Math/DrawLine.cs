using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class DrawLine : MonoBehaviour
{
    public Vector3 test;
    public float x;
    public float y;
    public float z;
    public float adjust1;
    public bool shoot = true;
    public float maxDistance = 1f;
    public Vector3 adjust2;

    public Transform Object;

    public void FixedUpdate()
    {
        Reflect();
    }

    public RaycastHit Reflect()
    {
        RaycastHit hit;
        Vector3 transformPoint = default;
        Ray ray = new Ray(transformPoint, Object.forward);
        Debug.DrawLine(new Vector3(x, y, z), Vector3.Reflect(Vector3.up, test), Color.black, 1f);
        Vector3 reflect = Vector3.Reflect(Vector3.up, test);
        Vector3 startPosition = new Vector3(0, 0, 0);
        transformPoint = Object.TransformPoint(adjust2);
        Debug.DrawLine(startPosition, reflect);
        Physics.Raycast(transform.position, -transform.up, out hit, maxDistance, 255, QueryTriggerInteraction.Ignore);
        Vector3.Reflect(adjust2, hit.normal);
        Debug.DrawLine(adjust2,Vector3.Reflect(Vector3.forward, test), Color.black, 2f);

        //Raycast();

        // Long version
        // Beginners tend to make more variable for each part as it's clearer.

        return hit;
    }
}

//public void Raycast()
    //{
        
            //Vector3 reflect       = Vector3.Reflect(Vector3.up, test);
            //Vector3 startPosition = new Vector3(x, y, z);
            //shoot = true;
            //Debug.DrawLine(new Vector3(x, y, z),Vector3.Reflect(Vector3.up, test), Color.black, 3f);
            //Debug.DrawLine(startPosition, reflect);
    //}


