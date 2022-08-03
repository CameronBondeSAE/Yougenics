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
    RaycastHit hit;
    
    public void FixedUpdate()
    {
        Debug.DrawLine(new Vector3(x, y, z), Vector3.Reflect(Vector3.up, test), Color.black, 1f);
        
        //Raycast();

        // Long version
        // Beginners tend to make more variable for each part as it's clearer.

    }

    //public void Raycast()
    //{
        
            //Vector3 reflect       = Vector3.Reflect(Vector3.up, test);
            //Vector3 startPosition = new Vector3(x, y, z);
            //shoot = true;
            //Debug.DrawLine(new Vector3(x, y, z),Vector3.Reflect(Vector3.up, test), Color.black, 3f);
            //Debug.DrawLine(startPosition, reflect);
    //}

}
