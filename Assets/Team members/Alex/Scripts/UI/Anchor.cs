using John;
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
            if (GetComponent<Anchor>() != null)
            {
                PlayerModel playerModel = NetworkManager.Singleton.LocalClient.PlayerObject.GetComponent<John.PlayerController>().playerModel;

                if(playerModel != null)
                    transform.LookAt(playerModel.transform);
                transform.Rotate(0,180,0,Space.Self);
            }
        }
    }
}
