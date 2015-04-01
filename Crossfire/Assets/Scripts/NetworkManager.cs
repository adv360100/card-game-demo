using UnityEngine;
using System.Collections;
using UnityEngine.UI;

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
	}
	
	void OnMasterServerEvent(MasterServerEvent msEvent)
	{
		switch (msEvent) {
		case MasterServerEvent.HostListReceived:
			Debug.Log ("Found " + MasterServer.PollHostList().Length + " games");
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
		networkView.RPC ("AddPlayer", RPCMode.OthersBuffered, new object[]{Network.player, ProfileManager.LoadPlayerInfo ().ToString()});
		networkView.RPC ("AddPlayerName", RPCMode.OthersBuffered, PersistantManager.GetInstance ().GetPlayerInfo (Network.player).Name);
		JoinLobby ();
	}

	void OnConnectedToServer() {
		Debug.Log("Connected to server");
		//todo: get others info
		PersistantManager.GetInstance().networkView.RPC ("AddPlayer", RPCMode.Server, new object[]{Network.player,  ProfileManager.LoadPlayerInfo ().ToString()});
		JoinLobby ();
	}

	void JoinLobby () {
		SM.OpenPanel (Lobby.LobbyController);
		Lobby.SetupLobby (Network.isServer);
		GetComponent<PlayersPanel> ().AddPlayerName (PersistantManager.GetInstance ().GetPlayerInfo (Network.player).Name);
		if (Network.isClient)
			networkView.RPC ("AddPlayerName", RPCMode.Others, PersistantManager.GetInstance ().GetPlayerInfo (Network.player).Name);
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
		PersistantManager.GetInstance ().RemovePlayer (player);
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
	void GetPlayerList(byte[] Namesdata)
	{

	}



	[RPC]
	void SetupLobby(string title)
	{
		Lobby.LobbyNameText.text = title;
	}
}
