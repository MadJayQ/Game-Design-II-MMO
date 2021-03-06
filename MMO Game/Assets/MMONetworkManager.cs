﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.Assertions;

using UnityEngine.Networking.NetworkSystem;

[Serializable]
public class NewPlayerMessage : MessageBase {
	public String PlayerName;
}
public class MMONetworkManager : NetworkManager {

	[SerializeField] private bool m_DebugServer = false;

	private static readonly string[] TEST_PARAMS = {
		"MMO Game.exe", 
		"-id",
		"0"
	};

	public ConsoleWindowConfiguration ConsoleConfiguration;

	[SerializeField] private ClientSettings m_ClientSettings;
	[SerializeField] private ServerSettings m_ServerSettings;

	public int RoomID = -1;

	public String username;

	public override void OnClientConnect(NetworkConnection conn) {
		NewPlayerMessage msg = new NewPlayerMessage();
		msg.PlayerName = username;
		var res = ClientScene.AddPlayer(conn, 0, msg);	
	}

	public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId, NetworkReader extraMessage) {
		var ap = extraMessage.ReadString();
		NewPlayerMessage newPlayerMsg = new NewPlayerMessage();
		if(newPlayerMsg == null) {
			return;
		}

		Transform startPos = GetStartPosition();
		GameObject playerObject = Instantiate(playerPrefab, startPos.position, startPos.rotation);
		if(playerObject != null) {
			MMOPlayer player = playerObject.GetComponent<MMOPlayer>();
			player.PlayerName = ap;

			NetworkServer.AddPlayerForConnection(
				conn,
				playerObject,
				playerControllerId
			);
		}
	}

	public override void OnClientSceneChanged(NetworkConnection conn) {

	}
	private void InitializeClient() {
		var defaultConfig = m_ServerSettings.ServerRoomConfigurations[0];
		offlineScene = m_ClientSettings.OfflineScene;
		onlineScene = defaultConfig.HostOnlineScene;
		networkAddress = defaultConfig.HostAddress;
		networkPort = defaultConfig.Port;
	}

	private void InitializeServer(ServerRoom roomConfiguration) {
		ConsoleConfiguration.Initailize();
		
		//Fill in our Network Manager Details from our configuration
		networkPort = roomConfiguration.Port;
		networkAddress = roomConfiguration.HostAddress;
		offlineScene = m_ClientSettings.OfflineScene;
		onlineScene = roomConfiguration.HostOnlineScene;

		StartServer();

		Debug.Log("The server has successfully started!");
	}

	void Start() {
		DontDestroyOnLoad(gameObject);

		var args = (m_DebugServer) ? TEST_PARAMS : System.Environment.GetCommandLineArgs();
		for(var i = 0; i < args.Length; i++) {
			var arg = args[i];
			if(arg == "-id") {
				if(i + 1 > args.Length) {
					Assert.IsTrue(false, "Invalid command paramaters");
				} else {
					if(!Int32.TryParse(args[i+1], out RoomID)) {
						RoomID = -1;
						Assert.IsTrue(false, "Invalid ID");
					}
				}
			}
		}
		if(RoomID == -1) { //We're a client, not a server
			InitializeClient();
		}
		else {
			var roomID = (int)RoomID;
			Assert.IsTrue(
				roomID < m_ServerSettings.ServerRoomConfigurations.Count, 
				"Invalid Room Configuration Setup"
			);
			InitializeServer(
				m_ServerSettings.ServerRoomConfigurations[roomID]
			);
		}
	}

	private IEnumerator BeamPlayer(int serverID) {
		StopClient();
		yield return new WaitForSeconds(1.0f);
		var config = m_ServerSettings.ServerRoomConfigurations[serverID];
		networkPort = config.Port;
		onlineScene = config.HostOnlineScene;

		StartClient();
	}
	public void MoveServer(int src, int dst) {
		StartCoroutine(BeamPlayer(dst));
	}
}
