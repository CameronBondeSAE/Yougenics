using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using TMPro;
using UnityEngine.UI;
using Unity.Collections;
using System;

public class LobbyManager : NetworkBehaviour
{
    public Canvas lobby;
    public TMP_Text clientUI;

    public string clientName;

    void OnGUI()
    {
        GUILayout.BeginArea(new Rect(10, 10, 300, 300));
        if (!NetworkManager.Singleton.IsClient && !NetworkManager.Singleton.IsServer)
        {
            StartButtons();
        }
        else
        {
            StatusLabels();
        }

        GUILayout.EndArea();
    }

    static void StartButtons()
    {
        if (GUILayout.Button("Host"))
        {
            NetworkManager.Singleton.StartHost();
        }

        if (GUILayout.Button("Client")) NetworkManager.Singleton.StartClient();
        if (GUILayout.Button("Server")) NetworkManager.Singleton.StartServer();
    }

    static void StatusLabels()
    {
        var mode = NetworkManager.Singleton.IsHost ?
            "Host" : NetworkManager.Singleton.IsServer ? "Server" : "Client";

        GUILayout.Label("Transport: " +
            NetworkManager.Singleton.NetworkConfig.NetworkTransport.GetType().Name);
        GUILayout.Label("Mode: " + mode);
    }

    private void Start()
    {
        
    }

    public override void OnNetworkSpawn()
    {
        NetworkManager.Singleton.OnClientConnectedCallback += OnClientJoin;
        NetworkManager.Singleton.OnClientDisconnectCallback += OnClientLeave;
    }

    private void OnClientLeave(ulong obj)
    {
        if(NetworkManager.Singleton.IsServer)
            HandleClientNames();
    }


    public void OnClientJoin(ulong clientID)
    {
        //Hack to work around Unity Calling this event before server is started
        if(!IsOwner)
        {
            StartCoroutine(OnClientJoinCoroutine());
        }
        else
        {
            HandleClientNames();
        }
    }

    public IEnumerator OnClientJoinCoroutine()
    {
        yield return new WaitForSeconds(2f);

        if (IsOwner)
        {
            HandleClientNames();
        }
    }

    void HandleClientNames()
    {
        clientName = "";

        foreach (NetworkClient client in NetworkManager.Singleton.ConnectedClientsList)
        {
            clientName += client.PlayerObject.GetComponent<ClientInfo>().name;
        }

        UpdateLobbyClientRPC(clientName);
    }

    [ClientRpc]
    public void UpdateLobbyClientRPC(string name)
    {
        clientUI.text = name;
    }
}
