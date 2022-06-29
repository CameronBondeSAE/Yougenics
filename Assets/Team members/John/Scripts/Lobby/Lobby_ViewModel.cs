using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.Netcode;
using System;

public class Lobby_ViewModel : MonoBehaviour
{
    public LobbyManager lobbyManager;
    NetworkClient client;

    public TMP_InputField inputField;

    // Start is called before the first frame update
    void Start()
    {
        lobbyManager.onLocalClientJoinEvent += HandleLocalClient;
    }

    private void HandleLocalClient(NetworkClient obj)
    {
        client = obj;
    }

    public void UpdateClientName()
    {
        if (client != null)
        {
            client.PlayerObject.GetComponent<ClientInfo>().clientName = inputField.text;
            HandleClientNamesReqServerRpc();
        }

        Debug.Log(inputField.text);
    }

    [ServerRpc]
    void HandleClientNamesReqServerRpc()
    {
        lobbyManager.HandleClientNames();
    }
}
