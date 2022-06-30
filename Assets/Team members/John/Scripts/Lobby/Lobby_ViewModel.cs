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
    NetworkObject player;

    public TMP_InputField inputField;

    // Start is called before the first frame update
    void Start()
    {
        lobbyManager.onLocalClientJoinEvent += HandleLocalClient;
    }

    private void HandleLocalClient(NetworkObject obj)
    {
        player = obj;
    }

    public void UpdateClientName()
    {
        if (player != null)
        {
            player.GetComponent<ClientInfo>().clientName = inputField.text;
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
