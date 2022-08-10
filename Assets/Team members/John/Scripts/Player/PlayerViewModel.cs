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
        if (playerModel.myClientInfo != null)
        {
            playerModel.myClientInfo.onNameChangeEvent += UpdateNameTag;
            nameTagText.text = playerModel.myClientInfo.ClientName.Value.ToString();
        }
        else
            Debug.Log("No Client Reference Found");
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
