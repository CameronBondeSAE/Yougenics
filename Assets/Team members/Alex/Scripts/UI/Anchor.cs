using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace Alex
{
    public class Anchor : MonoBehaviour
    {
        
        void LateUpdate()
        {
            transform.LookAt(NetworkManager.Singleton.LocalClient.PlayerObject.GetComponent<John.PlayerController>().playerModel.transform);
            //Hack
            transform.Rotate(0,180,0,Space.Self);
        }
    }
}
