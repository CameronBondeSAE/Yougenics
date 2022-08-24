using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnVision : MonoBehaviour
{
    public float TestDistance;
    public Transform targetObj;
    public Transform tresspasser;

    // Update is called once per frame
    public bool Refresh()
    {
        RaycastHit HitInfo;
        targetObj = null;


        Physics.Linecast(transform.position, tresspasser.position, out HitInfo);

        if(HitInfo.transform == tresspasser)
        {
            Debug.DrawLine(transform.position, HitInfo.transform.position, Color.red);
            targetObj = HitInfo.transform;
            return true;
        }
        else
        {
            Debug.DrawLine(transform.position, HitInfo.transform.position, Color.cyan);
            return false;
        }
        
    }

    /*private void Update()
    {
        RaycastHit HitInfo;
        targetObj = null;


        if (Physics.Raycast(transform.position, Vector3.forward, out HitInfo))
        {
            //Real code
            targetObj = HitInfo.transform;

            //Debugging or testing things
            Debug.DrawLine(transform.position, HitInfo.transform.position, Color.blue);
            TestDistance = HitInfo.distance;

            return true;
        }
        else
        {
            return false;
        }

        Physics.Linecast(transform.position, tresspasser.position, out HitInfo);

        if (HitInfo.transform == tresspasser)
        {
            Debug.DrawLine(transform.position, HitInfo.transform.position, Color.red);
            targetObj = HitInfo.transform;
            
        }
        else
        {
            Debug.DrawLine(transform.position, HitInfo.transform.position, Color.blue);
        }
    }*/


}
