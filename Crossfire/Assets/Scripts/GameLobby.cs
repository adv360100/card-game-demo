using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameLobby : MonoBehaviour {

	public Text LobbyNameText;
	public Animator LobbyController;
	public Text ActionButtonText;
	public int GameStartCountdown = 3;
	public GameObject RolesSection;
	public GameObject RacesSection;

	private Toggle[] RolesToggleArray;
	private Toggle[] RacesToggleArray;
	private string ReadyStr = "Ready";
	private string UnreadyStr = "Unready";
	private string StartStr = "Start";
	private bool PlayerIsReady = false;

	// Use this for initialization
	void Start () {
		RolesToggleArray = RolesSection.GetComponentsInChildren<Toggle> (true);//include inactive
		RacesToggleArray = RacesSection.GetComponentsInChildren<Toggle> (true);//include inactive

	}

	public void SetRace(int i)
	{
		//PersistantManager.GetInstance().SetRole(i);
		if (Network.isServer)
			SetRace (i, Network.player);
		else
			networkView.RPC ("SetRace", RPCMode.Server, new object[]{i,Network.player});
	}

	public void SetRole(int i)
	{
		//PersistantManager.GetInstance().SetRole(i);
		if (Network.isServer)
			SetRole (i, Network.player);
		else
			networkView.RPC ("SetRole", RPCMode.Server, new object[]{i,Network.player});
	}
	
	public void UpdateGroup(Toggle[] array)
	{
		
	}

	public void SetupLobby(bool isServer)
	{
		if (isServer) {
			ActionButtonText.text = StartStr;
		} else {
			ActionButtonText.text = ReadyStr;
			PlayerIsReady = false;
		}
	}

	public void PressedActionButton()
	{
		if (Network.isServer) {
			//ready the host
			PersistantManager.PlayerInfo p = PersistantManager.GetInstance().GetPlayerInfo(Network.player);
			p.IsReady = true;

			//do nothing if players are not ready
			bool ready = PersistantManager.GetInstance().Players.TrueForAll(delegate(PersistantManager.PlayerInfo obj) {
				return obj.IsReady;
			});

			//if all is ready then start count down
			if(ready)
			{
				Application.LoadLevel(1);
				return;
			}else{

			}
		} else {
			//toggle ready
			PlayerIsReady = !PlayerIsReady;
			networkView.RPC("SetPlayerReady",RPCMode.Server,new object[]{PlayerIsReady});
			if (PlayerIsReady) {
				ActionButtonText.text = UnreadyStr;
			} else {
				ActionButtonText.text = ReadyStr;
			}
		}

	}
	
	[RPC]
	void SetPlayerReady(bool isReady,NetworkMessageInfo info)
	{
		PersistantManager.PlayerInfo p = PersistantManager.GetInstance().GetPlayerInfo(info.sender);
		p.IsReady = !p.IsReady;
	}

	[RPC]
	void SetRole(int role, NetworkPlayer player)
	{
		PersistantManager.PlayerInfo p = PersistantManager.GetInstance ().GetPlayerInfo (player);
		p.Role = PersistantManager.Roles.RoleUnknown + role;
	}

	[RPC]
	void SetRace(int race, NetworkPlayer player)
	{
		PersistantManager.PlayerInfo p = PersistantManager.GetInstance ().GetPlayerInfo (player);
		p.Race = PersistantManager.Races.RaceUnknown + race;
	}
}
