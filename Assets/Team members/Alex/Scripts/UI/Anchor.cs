using John;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace Alex
{
    public class Anchor : MonoBehaviour
    {
        PlayerModel playerModel;

        private void Start()
        {
            if (NetworkManager.Singleton != null) playerModel = NetworkManager.Singleton.LocalClient.PlayerObject.GetComponent<John.PlayerController>().playerModel;
        }

        void LateUpdate()
        {

            if (playerModel != null)
                transform.LookAt(playerModel.transform);

            transform.Rotate(0, 180, 0, Space.Self);
        }
    }
}
