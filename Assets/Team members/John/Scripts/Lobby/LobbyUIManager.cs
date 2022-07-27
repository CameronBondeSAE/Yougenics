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
    [Header("Testing Setup")]
    public bool autoHost;
    public bool autoLoadLevel;
    public string sceneToLoad;

    [Header("Lobby UI Setup")]
    public GameObject lobbyCanvas;
    public TMP_Text clientUI;
    public Button startButton;
    public Button lobbyButton;
    public TMP_InputField playerNameField;

    [Header("IP Canvas Setup")]
    public GameObject ipAddressCanvas;
    public TMP_InputField serverIPInputField;

    [Header("Hack for now/Ignore")]
    public GameObject player;
    public GameObject lobbyCam;
    bool inGame = false;
    public GameObject lukeAITest;

    ulong myLocalClientId;
    NetworkObject myLocalClient;
    string clientName;

    public static LobbyUIManager instance;

    #region NetworkButtons/Status Info

    public void HostGame()
    {
        NetworkManager.Singleton.StartHost();

        if(!autoHost)
        {
            lobbyCanvas.SetActive(true);
            ipAddressCanvas.SetActive(false);
        }
        else
        {
            //spawn a player
            GameObject tempPlayer = Instantiate(player);

            //set ownership
            tempPlayer.GetComponent<NetworkObject>().SpawnWithOwnership(myLocalClientId);

            //Posses that player object
            myLocalClient.GetComponent<John.PlayerController>().playerModel = tempPlayer.GetComponent<PlayerModel>();
        }
    }

    public void JoinGame()
    {
        NetworkManager.Singleton.StartClient();
        //lobbyCanvas.GetComponentInChildren<Button>().gameObject.SetActive(false);
        startButton.gameObject.SetActive(false);
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
        if(!autoHost)
        {
            ipAddressCanvas.SetActive(true);
            lobbyCanvas.SetActive(false);
        }

        instance = this;
    }

    private void Start()
    {
        NetworkManager.Singleton.OnClientConnectedCallback += OnClientJoin;
        NetworkManager.Singleton.OnClientDisconnectCallback += OnClientLeave;

        serverIPInputField.text = NetworkManager.Singleton.GetComponent<UNetTransport>().ConnectAddress;

        if(autoHost)
        {
            HostGame();
        }

        if(autoLoadLevel)
        {
            StartGame();
        }
    }

    public void OnNewServerIPAddress()
    {
        NetworkManager.Singleton.GetComponent<UNetTransport>().ConnectAddress = serverIPInputField.text;
    }

    #region Handle Clients Joining/Leaving

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

    #endregion

    #region Handle Client Names/Lobby UI
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
    #endregion

    public void StartGame()
    {
        if(sceneToLoad == "")
        {
            Debug.Log("Select a level to load!");
            return;
        }

        NetworkManager.Singleton.SceneManager.OnSceneEvent += SceneManagerOnOnSceneEvent;

        //Update UI
        lobbyCam.SetActive(false);
        InGameLobbyUI(true);
        
        //Load the selected scene
        NetworkManager.Singleton.SceneManager.LoadScene(sceneToLoad, LoadSceneMode.Additive);

        //TODO: use this to know when the scene IS loaded - ligting doesn't load when this is called?
        //NetworkManager.Singleton.SceneManager.OnLoadComplete += OnLevelLoaded;
    }

    /*private void OnLevelLoaded(ulong clientId, string sceneName, LoadSceneMode loadSceneMode)
    {
        NetworkManager.Singleton.SceneManager.OnLoadComplete -= OnLevelLoaded;

        SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneName));
    }*/

    private void SceneManagerOnOnSceneEvent(SceneEvent sceneEvent)
    {
        NetworkManager.Singleton.SceneManager.OnSceneEvent -= SceneManagerOnOnSceneEvent;
        Scene scene = sceneEvent.Scene;

        //BUG: Scene is not yet loaded when this is called
        //SceneManager.SetActiveScene(scene);

        SubmitLobbyUIStateClientRpc(false);
        
        //TODO: Refactor this out of this script?
        //Spawn a player for each client
        foreach(NetworkClient client in NetworkManager.Singleton.ConnectedClientsList)
        {
            //spawn a player
            GameObject tempPlayer = Instantiate(player);

            //set ownership
            tempPlayer.GetComponent<NetworkObject>().SpawnWithOwnership(client.ClientId);

            //Posses that player object
            client.PlayerObject.GetComponent<John.PlayerController>().playerModel = tempPlayer.GetComponent<PlayerModel>();
        }

        //Spawn Luke AI For Testing
        GameObject lukeAI = Instantiate(lukeAITest);
        lukeAI.GetComponent<NetworkObject>().Spawn();
    }

    [ClientRpc]
    public void SubmitLobbyUIStateClientRpc(bool value)
    {
        lobbyCanvas.SetActive(value);
    }

    //Using this for when a client wants to display lobby during a game
    public void DisplayLobby(bool display)
    {
        lobbyCanvas.SetActive(display);
    }

    public void ReturnToLobby()
    {
        //Load lobby

        //unload active scene (HACK: Having issues setting the active scene on scene loaded)
        NetworkManager.Singleton.SceneManager.UnloadScene(SceneManager.GetSceneByName(sceneToLoad));

        //Update Lobby UI
        SubmitLobbyUIStateClientRpc(true);
        InGameLobbyUI(false);

        //Despawn Player Objects?
        foreach (NetworkClient client in NetworkManager.Singleton.ConnectedClientsList)
        {
            client.PlayerObject.GetComponent<John.PlayerController>().OnPlayerUnassigned();

            //client.PlayerObject.GetComponent<John.PlayerController>().playerModel.GetComponent<NetworkObject>().Despawn();

            /*foreach(NetworkObject obj in client.OwnedObjects)
            {
                obj.Despawn();
            }*/
        }
    }

    public void InGameLobbyUI(bool inGame)
    {
        if(inGame)
        {
            startButton.gameObject.SetActive(false);
            playerNameField.gameObject.SetActive(false);
            lobbyButton.gameObject.SetActive(true);
        }
        else
        {
            startButton.gameObject.SetActive(true);
            playerNameField.gameObject.SetActive(true);
            lobbyButton.gameObject.SetActive(false);
        }
    }
}
