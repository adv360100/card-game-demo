using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using SimpleJSON;

public class NetworkManager : MonoBehaviour {

	public InputField HostPort;
	public GameLobby Lobby;
	public GameHostList MasterGameListManager;

	private string GameTypeName = "ADVShadowrunCrossfireCoopDeckBuildingGame2520912987"; //unique ID
	private string GameName;
	private string Password = "";
	private int RemotePort = 25000;
	private int NumOfConnections = 4; //players
	private ScreenManager SM;

	// Use this for initialization
	void Start () {
		HostPort.text = RemotePort.ToString ();
		SM = GetComponent<ScreenManager> ();
	}

	public void LaunchServer() {
		//TODO check for vaild game name

		Network.incomingPassword = Password;
		bool useNat = !Network.HavePublicAddress();
		Network.InitializeServer(NumOfConnections, RemotePort, useNat);
		MasterServer.RegisterHost (GameTypeName, GameName,"This is a tech demo");
	}

	public static void ConnectToServer(HostData host,string password) {
		Network.Connect(host,password);
	}

	public void RefreshHostList()
	{
		MasterServer.RequestHostList(GameTypeName);
		Debug.Log ("Fetching game list");
		MasterGameListManager.ChangeState (GameHostList.JoinStates.JoinStateFetching);
	}
	
	void OnMasterServerEvent(MasterServerEvent msEvent)
	{
		switch (msEvent) {
		case MasterServerEvent.HostListReceived:
			Debug.Log ("Found " + MasterServer.PollHostList().Length + " games");
			if(MasterGameListManager != null)
				MasterGameListManager.AddHostGames(MasterServer.PollHostList ());
			break;
		case MasterServerEvent.RegistrationFailedNoServer:
			Debug.Log ("There is no server");
			break;
		}

	}

	void OnFailedToConnect(NetworkConnectionError error) {
		Debug.Log("Could not connect to server: " + error);
	}

	void OnServerInitialized() {
		Debug.Log("Server initialized and ready");
		networkView.RPC ("SetupLobby", RPCMode.AllBuffered, new object[]{GameName});
		PersistantManager.GetInstance ().AddPlayer (Network.player, ProfileManager.LoadPlayerInfo ());
		AddPlayer(Network.player, ProfileManager.LoadPlayerInfo ().ToString());
		JoinLobby ();
	}

	void OnConnectedToServer() {
		Debug.Log("Connected to server");
		networkView.RPC ("AddPlayer", RPCMode.All, new object[]{Network.player,  ProfileManager.LoadPlayerInfo ().ToString()});
		networkView.RPC ("QueryPlayers", RPCMode.Server, null);
		JoinLobby ();
	}

	[RPC]
	void QueryPlayers(NetworkMessageInfo info)
	{
		foreach (PersistantManager.PlayerInfo pi in PersistantManager.GetInstance().Players) {
			if(pi.Player != info.sender)
			{
				networkView.RPC ("AddPlayerWithName", info.sender, new object[]{pi.Player, pi.Name});
				networkView.RPC ("AddPlayerName", info.sender, pi.Name);
			}
		}
	}

	void JoinLobby () {
		SM.OpenPanel (Lobby.LobbyController);
		Lobby.SetupLobby (Network.isServer);
		string playerName = PersistantManager.GetInstance ().GetPlayerInfo (Network.player).Name;
		GetComponent<PlayersPanel> ().AddPlayerName (playerName);
		if (Network.isClient)
			networkView.RPC ("AddPlayerName", RPCMode.Others, new object[]{playerName});
	}

	void OnDisconnectedFromServer(NetworkDisconnection info) {
		if (Network.isServer)
			Debug.Log("Local server connection disconnected");
		else if (info == NetworkDisconnection.LostConnection)
				Debug.Log("Lost connection to the server");
		else
			Debug.Log("Successfully diconnected from the server");

		PersistantManager.GetInstance ().Players.Clear ();

		if(Network.isClient)
			RefreshHostList ();
		SM.OpenPreviousPanel();
	}

	void OnPlayerDisconnected(NetworkPlayer player) {
		Debug.Log("Clean up after player " + player);
		if (Lobby != null)
			Lobby.PlayerDroppedOut (player);
		networkView.RPC ("RemovePlayer", RPCMode.All, new object[]{player});
		Network.RemoveRPCs(player);
		Network.DestroyPlayerObjects(player);
	}

	public void DisconnectFromServer()
	{
		Network.Disconnect ();

		if (Network.isServer)
		{
			MasterServer.UnregisterHost();
		}
	}

	public void SetGameName(string n)
	{
		GameName = n;
	}

	public void SetPort(string p)
	{
		bool result = int.TryParse (p,out RemotePort);
		if (result == false)
			Debug.Log ("Failed to parse port number");
	}

	public void SetPassword(string p)
	{
		Password = p;
	}

	[RPC]
	public void AddPlayer (NetworkPlayer player, string json) {
		PersistantManager.GetInstance().AddPlayer (player, JSON.Parse (json));
	}

	[RPC]
	void AddPlayerWithName(NetworkPlayer player, string name)
	{
		PersistantManager.GetInstance ().AddPlayerWithName (player, name);
	}

	[RPC]
	public void RemovePlayer(NetworkPlayer player)
	{
		PersistantManager.GetInstance ().RemovePlayer (player);
	}

	[RPC]
	void SetupLobby(string title)
	{
		Lobby.LobbyNameText.text = title;
	}
}
