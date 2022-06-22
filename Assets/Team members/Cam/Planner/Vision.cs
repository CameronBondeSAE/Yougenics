using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Cam
{
    public class Vision : MonoBehaviour
    {
        public Transform target;
        
        public Transform thingICanSee;
        

        
        public bool RefreshVision()
        {
            thingICanSee = null;
            
            RaycastHit HitInfo;
            Physics.Linecast(transform.position, target.position, out HitInfo);

            // I hit the thing!
            if (HitInfo.transform == target)
            {
                thingICanSee = target;
                return true;
            }
            else
            {
                return false;
            }
        }
    }

}