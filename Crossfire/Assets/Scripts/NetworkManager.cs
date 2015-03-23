using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class NetworkManager : MonoBehaviour {

	public Text MyIPText;
	public InputField HostPort;
	public Text LobbyNameText;
	public Animator LobbyController;

	private string GameName;
	private string IPAddress;
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
		//bool useNat = !Network.HavePublicAddress();
		Network.InitializeServer(NumOfConnections, RemotePort, false);
	}

	public void ConnectToServer() {
		Network.Connect(IPAddress, RemotePort, Password);
	}

	void OnFailedToConnect(NetworkConnectionError error) {
		Debug.Log("Could not connect to server: " + error);
	}

	void OnServerInitialized() {
		Debug.Log("Server initialized and ready");
		JoinLobby ();
	}

	void OnConnectedToServer() {
		Debug.Log("Connected to server");
		JoinLobby ();
	}

	void JoinLobby()
	{
		LobbyNameText.text = GameName;
		SM.OpenPanel (LobbyController);
		PlayersPanel p = GetComponent<PlayersPanel> ();
		p.AddPlayer (PlayerName);
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

	public void SetIP(string i)
	{
		IPAddress = i;
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
}
