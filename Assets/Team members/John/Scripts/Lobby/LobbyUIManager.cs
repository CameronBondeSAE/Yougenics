using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using TMPro;
using UnityEngine.UI;
using Unity.Collections;
using System;
using UnityEngine.SceneManagement;
using Unity.Netcode.Transports.UNET;

public class LobbyUIManager : NetworkBehaviour
{
    public bool autoLoadLevel;

    public GameObject lobbyCanvas;
    public TMP_Text clientUI;
    public GameObject ipAddressCanvas;
    public TMP_InputField serverIPInputField;
    public TMP_InputField playerNameField;

    public string clientName;
    public GameObject player;
    public GameObject lobbyCam;

    ulong myLocalClientId;
    NetworkObject myLocalClient;

    #region NetworkButtons/Status Info

    public void HostGame()
    {
        NetworkManager.Singleton.StartHost();
        lobbyCanvas.SetActive(true);
        ipAddressCanvas.SetActive(false);
    }

    public void JoinGame()
    {
        NetworkManager.Singleton.StartClient();
        lobbyCanvas.GetComponentInChildren<Button>().gameObject.SetActive(false);
        lobbyCanvas.SetActive(true);
        ipAddressCanvas.SetActive(false);
    }

    public bool debugStatusLabels = true;
    void OnGUI()
    {
        GUILayout.BeginArea(new Rect(10, 10, 300, 300));
        if (NetworkManager.Singleton.IsClient && NetworkManager.Singleton.IsServer)
        {
            if(debugStatusLabels)
                StatusLabels();
        }

        GUILayout.EndArea();
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
        ipAddressCanvas.SetActive(true);
        lobbyCanvas.SetActive(false);
    }

    private void Start()
    {
        NetworkManager.Singleton.OnClientConnectedCallback += OnClientJoin;
        NetworkManager.Singleton.OnClientDisconnectCallback += OnClientLeave;

        serverIPInputField.text = NetworkManager.Singleton.GetComponent<UNetTransport>().ConnectAddress;
    }

    public void OnNewServerIPAddress()
    {
        NetworkManager.Singleton.GetComponent<UNetTransport>().ConnectAddress = serverIPInputField.text;
    }

    private void OnClientLeave(ulong obj)
    {
        //HACK to work around Unity Calling this event before ConnectedClientList is updated
        if (NetworkManager.Singleton.IsServer)
            Invoke("HandleClientNames", 1f);
    }


    public void OnClientJoin(ulong clientID)
    {
        if (NetworkManager.Singleton.IsServer || IsOwner)
        {
            HandleClientNames();
            HandleLocalClient(clientID);
        }

        if (clientID == NetworkManager.Singleton.LocalClientId)
            myLocalClientId = clientID;
    }

    void HandleLocalClient(ulong clientID)
    {
        NetworkClient tempClient;

        if (NetworkManager.Singleton.ConnectedClients.TryGetValue(clientID, out tempClient))
        {
            NetworkObject playerObject = tempClient.PlayerObject;
            if (playerObject.IsLocalPlayer)
            {
                myLocalClient = playerObject;
            }
        }
    }

    public void HandleClientNames()
    {
        clientName = "";

        for (int i = 0; i < NetworkManager.Singleton.ConnectedClientsList.Count; i++)
        {
            NetworkClient client = NetworkManager.Singleton.ConnectedClientsList[i];
            clientName += client.PlayerObject.GetComponent<ClientInfo>().clientName + " ";
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

    public void UpdateClientName()
    {
        if(IsServer)
        {
            if (myLocalClient != null)
            {
                myLocalClient.GetComponent<ClientInfo>().clientName = playerNameField.text;
                HandleClientNames();
            }
            else
            {
                Debug.Log("No local client found");
            }
        }
        else
        {
            RequestClientNameChangeServerRpc(myLocalClientId, playerNameField.text);
        }
    }

    [ServerRpc(RequireOwnership = false)]
    void RequestClientNameChangeServerRpc(ulong clientId, string name)
    {
        NetworkManager.Singleton.ConnectedClients[clientId].PlayerObject.GetComponent<ClientInfo>().clientName = name;
        HandleClientNames();
    }

    public void StartGame()
    {
        NetworkManager.Singleton.SceneManager.OnSceneEvent += SceneManagerOnOnSceneEvent;

        lobbyCam.SetActive(false);

        if(autoLoadLevel)
            NetworkManager.Singleton.SceneManager.LoadScene("JohnTestScene", LoadSceneMode.Additive);
    }

    private void SceneManagerOnOnSceneEvent(SceneEvent sceneEvent)
    {
        NetworkManager.Singleton.SceneManager.OnSceneEvent -= SceneManagerOnOnSceneEvent;
        Scene scene = sceneEvent.Scene;

        //SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
        //SceneManager.SetActiveScene(scene);

        lobbyCanvas.SetActive(false);
        
        SubmitLobbyUIStateClientRpc(false);
        
        
        //Spawn a player for each client
        foreach(NetworkClient client in NetworkManager.Singleton.ConnectedClientsList)
        {
            //spawn a player
            GameObject tempPlayer = Instantiate(player);

            //set ownership
            tempPlayer.GetComponent<NetworkObject>().SpawnWithOwnership(client.ClientId);

            //Posses that player object
            client.PlayerObject.GetComponent<John.PlayerController>().playerModel = tempPlayer.GetComponent<PlayerModel>();
            client.PlayerObject.GetComponent<John.PlayerController>().playerCameraModel = tempPlayer.GetComponentInChildren<PlayerCameraModel>();
        }
    }

    [ClientRpc]
    private void SubmitLobbyUIStateClientRpc(bool value)
    {
        lobbyCanvas.SetActive(value);
    }
}
