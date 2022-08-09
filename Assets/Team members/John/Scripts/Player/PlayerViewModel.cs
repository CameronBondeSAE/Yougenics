using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class PlayerViewModel : MonoBehaviour
{
    public PlayerModel playerModel;
    public TMP_Text nameTagText;

    private void Start()
    {
        //playerModel.onClientAssignedEvent += InitClientInfo;

        playerModel.myClientInfo.onNameChangeEvent += UpdateNameTag;
        nameTagText.text = playerModel.myClientInfo.ClientName.Value.ToString();
    }
    /*private void InitClientInfo(ClientInfo client)
    {
        client.onNameChangeEvent += UpdateNameTag;
        nameTagText.text = client.ClientName.Value.ToString();
    }*/

    private void UpdateNameTag(string newName)
    {
        nameTagText.text = newName;
    }
}
