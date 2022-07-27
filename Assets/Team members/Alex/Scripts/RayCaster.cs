using System;
using System.Collections;
using System.Collections.Generic;
using Alex;
using UnityEngine;

public class RayCaster : MonoBehaviour
{
    
    
    public bool canSpawn = true;

    private void Awake()
    {
        CheckForHeights();
    }

    private void Update()
    {
        CheckForHeights();
    }

    public void CheckForHeights()
    {

        RaycastHit hitInfo;
        hitInfo = new RaycastHit();

        if (Physics.Raycast(transform.position, transform.forward,
                out hitInfo, 2000))
        {
            if (Physics.Raycast(transform.position, transform.right,
                    out hitInfo, 2000))
            {


                Debug.DrawRay(transform.position, transform.forward * hitInfo.distance,
                    Color.red);
                Debug.DrawRay(transform.position, transform.right * hitInfo.distance,
                    Color.red);

                //Debug.Log("HIT");

                canSpawn = true;
            }

            else
            {
                canSpawn = false;
            }

        }
    }


}
