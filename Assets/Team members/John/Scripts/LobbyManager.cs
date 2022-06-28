using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using TMPro;
using UnityEngine.UI;
using Unity.Collections;
using System;
//using UnityEngine.SceneManagement;

public class LobbyManager : NetworkBehaviour
{
    public GameObject lobby;
    public TMP_Text clientUI;

    public string clientName;

    #region NetworkButtons/Status Info

    public bool debugStatusLabels = true;
    void OnGUI()
    {
        GUILayout.BeginArea(new Rect(10, 10, 300, 300));
        if (!NetworkManager.Singleton.IsClient && !NetworkManager.Singleton.IsServer)
        {
            StartButtons();
        }
        else
        {
            if(debugStatusLabels)
                StatusLabels();
        }

        GUILayout.EndArea();
    }

    public void StartButtons()
    {
        if (GUILayout.Button("Host"))
        {
            NetworkManager.Singleton.StartHost();
            lobby.SetActive(true);
        }

        if (GUILayout.Button("Client"))
        {
            NetworkManager.Singleton.StartClient();
            lobby.SetActive(true);
        }

        if (GUILayout.Button("Server"))
        {
            NetworkManager.Singleton.StartServer();
            lobby.SetActive(true);
        }
    }

    static void StatusLabels()
    {
        var mode = NetworkManager.Singleton.IsHost ?
            "Host" : NetworkManager.Singleton.IsServer ? "Server" : "Client";

        GUILayout.Label("Transport: " +
            NetworkManager.Singleton.NetworkConfig.NetworkTransport.GetType().Name);
        GUILayout.Label("Mode: " + mode);
    }

    #endregion

    private void Awake()
    {
        lobby.SetActive(false);
    }

    private void Start()
    {
        NetworkManager.Singleton.OnClientConnectedCallback += OnClientJoin;
        NetworkManager.Singleton.OnClientDisconnectCallback += OnClientLeave;
    }

    private void OnClientLeave(ulong obj)
    {
        //HACK to work around Unity Calling this event before ConnectedClientList is updated
        if (NetworkManager.Singleton.IsServer)
            Invoke("HandleClientNames", 1f);
    }


    public void OnClientJoin(ulong clientID)
    {
        if(NetworkManager.Singleton.IsServer || IsOwner)
        {
            HandleClientNames();
        }
    }

    public void HandleClientNames()
    {
        clientName = "";

        for (int i = 0; i < NetworkManager.Singleton.ConnectedClientsList.Count; i++)
        {
            clientName += NetworkManager.Singleton.ConnectedClientsList[i].PlayerObject.GetComponent<ClientInfo>().clientName + " " + (i + 1) + " ";
        }

        if (NetworkManager.Singleton.IsServer)
        {
            UpdateLobbyClientListName(clientName);
        }

        UpdateLobbyClientRPC(clientName);
    }

    [ClientRpc]
    public void UpdateLobbyClientRPC(string name)
    {
        UpdateLobbyClientListName(name);
    }

    public void UpdateLobbyClientListName(string _name)
    {
        clientUI.text = _name;
    }

    public void StartGame()
    {
        //Load Scene

    }
}
