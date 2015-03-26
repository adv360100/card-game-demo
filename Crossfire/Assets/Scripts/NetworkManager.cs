using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class NetworkManager : MonoBehaviour {

	public Text MyIPText;
	public InputField HostPort;
	public GameLobby Lobby;
	public GameHostList MasterGameListManager;

	private string GameTypeName = "ShadowrunCrossfireCoopDeckBuildingGame"; //unique ID
	private string GameName;
	private string Password = "";
	private string PlayerName;
	private int RemotePort = 25000;
	private int NumOfConnections = 4; //players
	private ScreenManager SM;

	// Use this for initialization
	void Start () {
		MyIPText.text = Network.player.ipAddress;
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
		string subtitle = Network.player.ipAddress + " port:" + RemotePort.ToString();
		networkView.RPC ("SetupLobby", RPCMode.AllBuffered, new object[]{GameName,subtitle});
		JoinLobby ();
	}

	void OnConnectedToServer() {
		Debug.Log("Connected to server");
		JoinLobby ();
	}

	void JoinLobby()
	{
		SM.OpenPanel (Lobby.LobbyController);
		if (Network.isServer)
			AddPlayer (PlayerName);
		else
			networkView.RPC ("AddPlayer", RPCMode.Server,PlayerName);
	}

	void OnDisconnectedFromServer(NetworkDisconnection info) {
		if (Network.isServer)
			Debug.Log("Local server connection disconnected");
		else
			if (info == NetworkDisconnection.LostConnection)
				Debug.Log("Lost connection to the server");
		else
			Debug.Log("Successfully diconnected from the server");

		SM.OpenPreviousPanel();
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

	public void SetPlayerName(string n)
	{
		PlayerName = n;
	}

	[RPC]
	void GetPlayerList(byte[] Namesdata)
	{

	}

	[RPC]
	void AddPlayer(string name)
	{
		PlayersPanel p = GetComponent<PlayersPanel> ();
		p.AddPlayer (name);

		//networkView.RPC ("GetPlayerList", RPCMode.All, ms.ToArray());
	}

	[RPC]
	void SetupLobby(string title, string subtitle)
	{
		Lobby.LobbyNameText.text = title;
		Lobby.IPText.text = subtitle;
	}
}
