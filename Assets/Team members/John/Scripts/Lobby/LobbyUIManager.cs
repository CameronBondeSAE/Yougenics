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
using DG.Tweening;

namespace John
{
	[Serializable]
	public class Level
	{
		public UnityEngine.Object level;
		public string             levelNameOnUI;
	}

	public class LobbyUIManager : NetworkBehaviour
	{
		[Header("Testing Setup")]
		public bool autoHost;

		public bool   autoLoadLevel;
		public bool   spawnPlayerOnAwake;
		public string sceneToLoad;

		[Header("Level Setup")]
		public List<Level> levels;

		public GameObject levelHolder;
		public GameObject levelButtonPrefab;
		public Spawner    critterSpawner;

		[Header("Lobby UI Setup")]
		public GameObject lobbyCanvas;

		public TMP_Text       clientUI;
		public Button         startButton;
		public Button         lobbyButton;
		public TMP_InputField playerNameField;
		public GameObject     levelSelectUI;
		public GameObject     waitForHostBanner;
		public GameObject     clientField;
		public GameObject     clientLobbyUIPrefab;

		[Header("IP Canvas Setup")]
		public GameObject ipAddressCanvas;

		public TMP_InputField serverIPInputField;

		[Header("Hack for now/Ignore")]
		public GameObject playerPrefab;
		//public Camera playerCamera;

		public GameObject lobbyCam;
		bool              inGame = false;
		public GameObject lukeAITest;
		[SerializeField]
		ulong         myLocalClientId;
		NetworkObject myLocalClient;
		string        clientName;

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
					levelButton.GetComponent<LevelButton>().myLevel     = level.level.name;
				}
			}
			else
			{
				//Turn Off Lobby - don't need it for quick testing
				lobbyCanvas.SetActive(false);

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
				if (debugStatusLabels)
					StatusLabels();
			}

			GUILayout.EndArea();
		}

		static void StatusLabels()
		{
			var mode = NetworkManager.Singleton.IsHost ? "Host" : NetworkManager.Singleton.IsServer ? "Server" : "Client";

			GUILayout.Label("Transport: " +
							NetworkManager.Singleton.NetworkConfig.NetworkTransport.GetType().Name);
			GUILayout.Label("Mode: " + mode);
		}

		#endregion

		private void Awake()
		{
			//Setup IP Address Canvas
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
			//NetworkManager.Singleton.OnClientDisconnectCallback += OnClientLeave;

			serverIPInputField.text = NetworkManager.Singleton.GetComponent<UNetTransport>().ConnectAddress;

			if (autoHost)
			{
				HostGame();
			}

			if (autoLoadLevel)
			{
				StartGame();
			}
		}

		public void OnNewServerIPAddress()
		{
			NetworkManager.Singleton.GetComponent<UNetTransport>().ConnectAddress = serverIPInputField.text;
		}

		#region Handle Clients Joining/Leaving

		/*private void OnClientLeave(ulong obj)
		{
			//HACK to work around Unity Calling this event before ConnectedClientList is updated
			if (NetworkManager.Singleton.IsServer)
			{
				Invoke("HandleClientNames", 1f);
			}
		}*/

		public void OnClientJoin(ulong clientID)
		{
			if (NetworkManager.Singleton.IsServer || IsOwner)
			{
				NetworkClient client;
				if (NetworkManager.Singleton.ConnectedClients.TryGetValue(clientID, out client))
				{
					ClientInfo clientInfo = client.PlayerObject.GetComponent<ClientInfo>();

					//Using the client count as the player number reference as clientID does not lower if a player leaves and rejoins (so player 2 ends up incrementing to player 3 etc)
					clientInfo.Init((ulong)NetworkManager.Singleton.ConnectedClients.Count);

					GameObject uiRef = Instantiate(clientLobbyUIPrefab, clientField.transform);
					clientInfo.lobbyUIRef               = uiRef;
					uiRef.GetComponent<TMP_Text>().text = clientInfo.ClientName.Value.ToString();
				}

				//HandleClientNames();
				HandleLocalClient(clientID);
			}
			else
				RequestClientNamesLobbyUIServerRpc(clientID);

			if (clientID == NetworkManager.Singleton.LocalClientId)
				myLocalClientId = clientID;
		}

		[ServerRpc(RequireOwnership = false)]
		public void RequestClientNamesLobbyUIServerRpc(ulong clientId)
		{
			//Spawning ClientLobbyUI
			foreach (NetworkClient client in NetworkManager.Singleton.ConnectedClientsList)
			{
				HandleNewClientsJoiningClientRpc(client.PlayerObject.GetComponent<ClientInfo>().ClientName.Value.ToString(), client.ClientId, clientId);
			}
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

			UpdateLobbyClientRPC(clientName);
		}

		//This handles NEW clients joining the server - Ensuring the lobby UI is accurate on all clients
		[ClientRpc]
		public void HandleNewClientsJoiningClientRpc(string clientName, ulong incommingClientId, ulong callerClientId)
		{
			//HACK: Stop duplicates on the server
			if (IsServer)
				return;

			//HACK: Doing this to still update lobby UI for existing clients when NEW clients join
			if (NetworkManager.Singleton.LocalClientId != callerClientId)
			{
				//SO we only care about spawning new lobby UI's if the incomming cient Id's are greater then the clients (as these should be clients that this client don't know about)
				if (incommingClientId > NetworkManager.Singleton.LocalClientId)
				{
					SpawnClientLobbyUI(clientName, incommingClientId);
				}

				return;
			}

			//Otherwise for the client that just joined - spawn a lobby UI for all the clients currently in the lobby including their own
			SpawnClientLobbyUI(clientName, incommingClientId);
		}

		[ServerRpc(RequireOwnership = false)]
		public void RequestClientUIUpdateServerRpc()
		{
			HandleClientNameChange();
		}

		void HandleClientNameChange()
		{
			//Removing all clientUI's to replace them with updated names
			//TODO: Find better way of doing this
			ClearLobbyNamesClientRpc();

			foreach (NetworkClient client in NetworkManager.Singleton.ConnectedClientsList)
			{
				SpawnClientLobbyUIClientRpc(client.PlayerObject.GetComponent<ClientInfo>().ClientName.Value.ToString(), client.ClientId);
			}
		}

		[ClientRpc]
		public void SpawnClientLobbyUIClientRpc(string newName, ulong incommingId)
		{
			SpawnClientLobbyUI(newName, incommingId);
		}

		void SpawnClientLobbyUI(string clientName, ulong incommingId)
		{
			GameObject uiRef = Instantiate(clientLobbyUIPrefab, clientField.transform);
			uiRef.GetComponent<TMP_Text>().text                                                     = clientName;

			if(incommingId == myLocalClientId)
				NetworkManager.Singleton.LocalClient.PlayerObject.GetComponent<ClientInfo>().lobbyUIRef = uiRef;
		}

		[ClientRpc]
		public void ClearLobbyNamesClientRpc()
		{
			foreach (Transform child in clientField.transform)
			{
				Destroy(child.gameObject);
			}
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
			if (IsServer)
			{
				if (myLocalClient != null)
				{
					myLocalClient.GetComponent<ClientInfo>().ClientName.Value = playerNameField.text;
					HandleClientNameChange();
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
			NetworkManager.Singleton.ConnectedClients[clientId].PlayerObject.GetComponent<ClientInfo>().ClientName.Value = name;
			HandleClientNameChange();
		}

        #endregion

        #region Handle Starting / Leaving Game

        public void StartGame()
		{
			if (sceneToLoad == "")
			{
				Debug.Log("Select a level to load!");
				levelSelectUI.transform.DOPunchScale(new Vector3(0.25f, 0.25f, 1f), 0.25f, 2, 0.1f);
				return;
			}

			//NetworkManager.Singleton.SceneManager.OnSceneEvent += SceneManagerOnOnSceneEvent;

			//BUG: use this to know when the scene IS loaded - ligting doesn't load when this is called?
			NetworkManager.Singleton.SceneManager.OnLoadComplete += OnLevelLoaded;

			//Load the selected scene
			//BUG: If this fails it will duplicate the spawns of the player
			try
			{
				NetworkManager.Singleton.SceneManager.LoadScene(sceneToLoad, LoadSceneMode.Additive);
			}
			catch (Exception e)
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
				GameObject            tempPlayer;
				John.PlayerController controller;

				//spawn a player
				if (spawnPoints.Length > 0)
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
				controller             = client.PlayerObject.GetComponent<John.PlayerController>();
				controller.playerModel = tempPlayer.GetComponent<PlayerModel>();

				//This only works on the server
				tempPlayer.GetComponent<PlayerModel>().myClientInfo = client.PlayerObject.GetComponent<ClientInfo>();
			}

			//Activate controller on all clients
			InitControllerClientRpc();

			critterSpawner.SpawnMultiple();

			foreach (GameObject spawnedSpawnerGO in critterSpawner.spawned)
			{
				spawnedSpawnerGO.GetComponent<Spawner>().SpawnMultiple();
			}

			//Update UI
			lobbyCam.SetActive(false);
			SubmitLobbyUIStateClientRpc(true);
		}

		[ClientRpc]
		public void InitControllerClientRpc()
		{
			NetworkObject         myClient;
			John.PlayerController controller;

			if (!IsServer)
			{
				myClient = NetworkManager.Singleton.LocalClient.PlayerObject;
			}
			else
				myClient = myLocalClient;

			controller = myClient.GetComponent<John.PlayerController>();
			controller.playerInput.ActivateInput();
			controller.playerInput.SwitchCurrentActionMap("InGame");
			controller.OnPlayerAssigned();

			//controller.playerModel.myClientInfo = myClient.GetComponent<ClientInfo>();
			//controller.playerModel.OnClientAssigned(myClient.GetComponent<ClientInfo>());
		}

		private void SceneManagerOnOnSceneEvent(SceneEvent sceneEvent)
		{
			NetworkManager.Singleton.SceneManager.OnSceneEvent -= SceneManagerOnOnSceneEvent;
			Scene scene = sceneEvent.Scene;

			//BUG: Scene is not yet loaded when this is called
			//SceneManager.SetActiveScene(scene);

			//Update UI
			/*lobbyCam.SetActive(false);
			SubmitLobbyUIStateClientRpc(true);*/
		}

		public void ReturnToLobby()
		{
			//Load lobby

			NetworkManager.Singleton.SceneManager.UnloadScene(SceneManager.GetSceneByName(sceneToLoad));
			critterSpawner.spawned.Clear();

			//Turn Off Controller on all clients
			ResetControllerClientRpc();

			//Update Lobby UI
			SubmitLobbyUIStateClientRpc(false);
			lobbyCam.SetActive(true);
		}

		[ClientRpc]
		public void ResetControllerClientRpc()
		{
			NetworkObject         myClient;
			John.PlayerController controller;

			if (!IsServer)
			{
				myClient = NetworkManager.Singleton.LocalClient.PlayerObject;
			}
			else
				myClient = myLocalClient;

			controller = myClient.GetComponent<John.PlayerController>();
			controller.OnPlayerUnassigned();
			controller.playerInput.DeactivateInput();
		}

        #endregion

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
			if (inGame)
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
}