using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using Unity.Netcode;

namespace John
{
    public class PlayerViewModel : NetworkBehaviour
    {
        public PlayerModel playerModel;
        public TMP_Text    nameTagText;
        ClientInfo         client;

        private void Start()
        {
            //playerModel.onClientAssignedEvent += InitClientInfo;

            if (playerModel.myClientInfo != null)
            {
                playerModel.myClientInfo.onNameChangeEvent += UpdateNameTag;
                nameTagText.text                           =  playerModel.myClientInfo.ClientName.Value.ToString();
            }
            else
            {
                Debug.Log("Cannot update NameTag, No Client Reference Found");
            }
        }
        /*private void InitClientInfo()
        {
            client = playerModel.myClientInfo;
            client.onNameChangeEvent += UpdateNameTag;
            nameTagText.text = client.ClientName.Value.ToString();
        }*/

        private void UpdateNameTag(string newName)
        {
            nameTagText.text = newName;
        }
    }
}