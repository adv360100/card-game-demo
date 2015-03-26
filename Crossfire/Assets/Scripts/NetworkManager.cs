using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class NetworkManager : MonoBehaviour {

	public InputField HostPort;
	public GameLobby Lobby;
	public GameHostList MasterGameListManager;

	private string GameTypeName = "ShadowrunCrossfireCoopDeckBuildingGame"; //unique ID
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
	
	// Update is called once per frame
	void Update () {
	
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
		PersistantManager.GetInstance ().AddPlayer ("Host", Network.player);
		JoinLobby ();
	}

	void OnConnectedToServer() {
		Debug.Log("Connected to server");
		networkView.RPC ("AddPlayer", RPCMode.Server, new object[]{"Player", Network.player});
		JoinLobby ();
	}

	void JoinLobby()
	{
		SM.OpenPanel (Lobby.LobbyController);
//		if (Network.isServer)
//			AddPlayer (PlayerName);
//		else
//			networkView.RPC ("AddPlayer", RPCMode.Server,PlayerName);
	}

	void OnDisconnectedFromServer(NetworkDisconnection info) {
		if (Network.isServer)
			Debug.Log("Local server connection disconnected");
		else if (info == NetworkDisconnection.LostConnection)
				Debug.Log("Lost connection to the server");
		else
			Debug.Log("Successfully diconnected from the server");

		SM.OpenPreviousPanel();
	}

	void OnPlayerDisconnected(NetworkPlayer player) {
		Debug.Log("Clean up after player " + player);
		PersistantManager.GetInstance ().RemovePlayer (player);
		Network.RemoveRPCs(player);
		Network.DestroyPlayerObjects(player);
	}

	public void DisconnectFromServer()
	{
		if (Network.isClient)
			Network.Disconnect ();
		else {
			Network.Disconnect();
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
	void AddPlayer(string name)
	{
		//PlayersPanel p = GetComponent<PlayersPanel> ();
		//p.AddPlayer (name);

		//networkView.RPC ("GetPlayerList", RPCMode.All, ms.ToArray());
	}

	[RPC]
	void SetupLobby(string title)
	{
		Lobby.LobbyNameText.text = title;
	}
}
