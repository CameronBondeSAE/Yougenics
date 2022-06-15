using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using TMPro;
using UnityEngine.UI;

public class LobbyManager : MonoBehaviour
{
    public Canvas lobby;
    public TMP_Text clientUI;

    public NetworkVariable<ulong> ClientID = new NetworkVariable<ulong>();

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
        NetworkManager.Singleton.OnClientConnectedCallback += OnClientJoin;

        ClientID.OnValueChanged += UpdateLobbyUI;
    }

    public void OnClientJoin(ulong clientID)
    {
        if(NetworkManager.Singleton.IsServer)
        {
            ClientID.Value = clientID;
        }
        else
        {
            SubmitLobbyRequestServerRpc(clientID);
        }
        
    }

    [ServerRpc]
    void SubmitLobbyRequestServerRpc(ulong clientID)
    {
        ClientID.Value = clientID;
    }

    public void UpdateLobbyUI(ulong previousValue, ulong newValue)
    {
        if (NetworkManager.Singleton.ConnectedClientsList.Count <= 0)
        {
            clientUI.text = NetworkManager.Singleton.SpawnManager.GetPlayerNetworkObject(newValue).GetComponent<ClientInfo>().name;
        }
        else
        {
            clientUI.text += NetworkManager.Singleton.SpawnManager.GetPlayerNetworkObject(newValue).GetComponent<ClientInfo>().name;
        }
    }
}
