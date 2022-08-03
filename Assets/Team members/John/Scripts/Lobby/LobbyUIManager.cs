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

[Serializable]
public class Level
{
    public UnityEngine.Object level;
    public string levelNameOnUI;
}

public class LobbyUIManager : NetworkBehaviour
{
    [Header("Testing Setup")]
    public bool autoHost;
    public bool autoLoadLevel;
    public bool spawnPlayerOnAwake;
    public string sceneToLoad;

    [Header("Level Setup")]
    public List<Level> levels;
    public GameObject levelHolder;
    public GameObject levelButtonPrefab;

    [Header("Lobby UI Setup")]
    public GameObject lobbyCanvas;
    public TMP_Text clientUI;
    public Button startButton;
    public Button lobbyButton;
    public TMP_InputField playerNameField;
    public GameObject levelSelectUI;
    public GameObject waitForHostBanner;
    //public GameObject clientField;
    //public GameObject clientLobbyUIPrefab;

    [Header("IP Canvas Setup")]
    public GameObject ipAddressCanvas;
    public TMP_InputField serverIPInputField;

    [Header("Hack for now/Ignore")]
    public GameObject playerPrefab;
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

        if (!autoHost)
        {
            lobbyCanvas.SetActive(true);
            ipAddressCanvas.SetActive(false);

            //Dynamically Set Up Level Selector
            foreach (Level level in levels)
            {
                GameObject levelButton = Instantiate(levelButtonPrefab, levelHolder.transform);
                levelButton.GetComponentInChildren<TMP_Text>().text = level.levelNameOnUI;
                levelButton.GetComponent<LevelButton>().myLevel = level.level.name;
            }
        }
        else
        {
            if (!spawnPlayerOnAwake)
                return;

            //spawn a player
            GameObject tempPlayer = Instantiate(playerPrefab);

            //set ownership
            tempPlayer.GetComponent<NetworkObject>().SpawnWithOwnership(myLocalClientId);

            //Posses that player object
            myLocalClient.GetComponent<John.PlayerController>().playerModel = tempPlayer.GetComponent<PlayerModel>();
        }
    }

    public void JoinGame()
    {
        NetworkManager.Singleton.StartClient();

        SetupClientUI();
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
        if (!autoHost)
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
        {
            Invoke("HandleClientNames", 1f);
        }
    }

    public void OnClientJoin(ulong clientID)
    {
        if (NetworkManager.Singleton.IsServer || IsOwner)
        {
            //Using the client count as the player number reference as clientID does not lower if a player leaves and rejoins (so player 2 ends up incrementing to player 3 etc)
            NetworkClient client;
            if (NetworkManager.Singleton.ConnectedClients.TryGetValue(clientID, out client))
            {
                ClientInfo clientInfo = client.PlayerObject.GetComponent<ClientInfo>();
                clientInfo.Init((ulong)NetworkManager.Singleton.ConnectedClients.Count);

                //Ignore----------------------------------------------------------------
                //GameObject uiRef = Instantiate(clientLobbyUIPrefab, clientField.transform);
                /*clientInfo.lobbyUIRef = uiRef;
                uiRef.GetComponent<TMP_Text>().text = clientInfo.clientName;*/
                //SpawnClientLobbyUIClientRpc();
            }

            HandleClientNames();
            HandleLocalClient(clientID);
        }

        if (clientID == NetworkManager.Singleton.LocalClientId)
            myLocalClientId = clientID;
    }

    //-----------------------------
    //IGNORE: Was trying to get spawning a clientLobbyUIPrefab working

    /*[ServerRpc(RequireOwnership = false)]
    private void ReqClientLobbyUIServerRpc()
    {
        SpawnClientLobbyUIClientRpc();
    }

    [ClientRpc]
    public void SpawnClientLobbyUIClientRpc()
    {
        if (!IsServer)
            Instantiate(clientLobbyUIPrefab, clientField.transform);

        //Only do this on clients to prevent duplicate spawnings on the server

        foreach (NetworkClient client in NetworkManager.Singleton.ConnectedClientsList)
        {
            if (!IsServer)
                Instantiate(client.PlayerObject.GetComponent<ClientInfo>().lobbyUIRef, clientField.transform);
            Debug.Log("Test");
        }
    }*/
    //-----------------------------

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

        //BUG: use this to know when the scene IS loaded - ligting doesn't load when this is called?
        NetworkManager.Singleton.SceneManager.OnLoadComplete += OnLevelLoaded;

        //Load the selected scene
        //BUG: If this fails it will duplicate the spawns of the player
        try
        {
            NetworkManager.Singleton.SceneManager.LoadScene(sceneToLoad, LoadSceneMode.Additive);
        }
        catch(Exception e)
        {
            Debug.LogException(e, this);
        }

    }

    private void OnLevelLoaded(ulong clientId, string sceneName, LoadSceneMode loadSceneMode)
    {
        NetworkManager.Singleton.SceneManager.OnLoadComplete -= OnLevelLoaded;

        SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneName));

        SpawnPoint[] spawnPoints = FindObjectsOfType<SpawnPoint>();

        //TODO: Refactor this out of this script?
        //Spawn a player for each client
        foreach (NetworkClient client in NetworkManager.Singleton.ConnectedClientsList)
        {
            GameObject tempPlayer;

            //spawn a player
            if(spawnPoints.Length > 0)
            {
                SpawnPoint randomSpawn = spawnPoints[UnityEngine.Random.Range(0, spawnPoints.Length)];
                tempPlayer = Instantiate(playerPrefab, randomSpawn.transform.position, Quaternion.identity);
            }
            else
            {
                tempPlayer = Instantiate(playerPrefab);
                Debug.Log("No Spawn Points Found");
            }

            //set ownership
            tempPlayer.GetComponent<NetworkObject>().SpawnWithOwnership(client.ClientId);

            //Posses that player object
            client.PlayerObject.GetComponent<John.PlayerController>().playerModel = tempPlayer.GetComponent<PlayerModel>();
        }

        //Spawn Luke AI For Testing
        GameObject lukeAI = Instantiate(lukeAITest);
        lukeAI.GetComponent<NetworkObject>().Spawn();
    }

    private void SceneManagerOnOnSceneEvent(SceneEvent sceneEvent)
    {
        NetworkManager.Singleton.SceneManager.OnSceneEvent -= SceneManagerOnOnSceneEvent;
        Scene scene = sceneEvent.Scene;

        //BUG: Scene is not yet loaded when this is called
        //SceneManager.SetActiveScene(scene);

        //Update UI
        lobbyCam.SetActive(false);
        SubmitLobbyUIStateClientRpc(true);
    }

    public void ReturnToLobby()
    {
        //Load lobby

        //Despawn Player Objects?
        /*foreach (NetworkClient client in NetworkManager.Singleton.ConnectedClientsList)
        {
            client.PlayerObject.GetComponent<John.PlayerController>().OnPlayerUnassigned();

            //client.PlayerObject.GetComponent<John.PlayerController>().playerModel.GetComponent<NetworkObject>().Despawn();

            *//*foreach(NetworkObject obj in client.OwnedObjects)
            {
                obj.Despawn();
            }*//*
        }*/

        //unload active scene (HACK: Having issues setting the active scene on scene loaded)
        NetworkManager.Singleton.SceneManager.UnloadScene(SceneManager.GetSceneByName(sceneToLoad));

        //Update Lobby UI
        SubmitLobbyUIStateClientRpc(false);
        lobbyCam.SetActive(true);
    }

    #region UI Hacky Code
    [ClientRpc]
    public void SubmitLobbyUIStateClientRpc(bool inGame)
    {
        lobbyCanvas.SetActive(!inGame);

        InGameLobbyUI(inGame);
    }

    //Using this for when a client wants to display lobby during a game
    public void DisplayLobby(bool display)
    {
        lobbyCanvas.SetActive(display);
    }
    public void InGameLobbyUI(bool inGame)
    {
        if(inGame)
        {
            playerNameField.gameObject.SetActive(false);

            if (IsServer)
            {
                levelSelectUI.SetActive(false);
                lobbyButton.gameObject.SetActive(true);
                startButton.gameObject.SetActive(false);
            }
            else
                waitForHostBanner.SetActive(false);
        }
        else
        {
            playerNameField.gameObject.SetActive(true);

            if (IsServer)
            {
                lobbyButton.gameObject.SetActive(false);
                levelSelectUI.SetActive(true);
                startButton.gameObject.SetActive(true);
            }
            else
                waitForHostBanner.SetActive(true);
        }
    }

    void SetupClientUI()
    {
        startButton.gameObject.SetActive(false);
        lobbyCanvas.SetActive(true);
        ipAddressCanvas.SetActive(false);
        levelSelectUI.SetActive(false);
        waitForHostBanner.SetActive(true);
    }
    #endregion
}
