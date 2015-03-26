using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameLobby : MonoBehaviour {

	public Text LobbyNameText;
	public Animator LobbyController;
	public Text ActionButton;
	public int GameStartCountdown = 3;
	public GameObject RolesSection;
	public GameObject RacesSection;

	private Toggle[] RolesToggleArray;
	private Toggle[] RacesToggleArray;

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

	public void PressedActionButton()
	{
		if (Network.isServer) {
			//do nothing if players are not ready
			bool ready = PersistantManager.GetInstance().Players.TrueForAll(delegate(PersistantManager.PlayerInfo obj) {
				return obj.IsReady;
			});

			//if all is ready then start count down
			if(ready)
			{

			}else{

			}
		} else {
			//toggle ready
		}

	}

	[RPC]
	void PlayerIsReady(bool isReady)
	{

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
