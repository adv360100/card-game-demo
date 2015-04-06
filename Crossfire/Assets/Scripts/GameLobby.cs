using UnityEngine;
using System.Collections;
using System.Collections.Generic;
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
	void Awake () {
		RolesToggleArray = RolesSection.GetComponentsInChildren<Toggle> (true);//include inactive
		RacesToggleArray = RacesSection.GetComponentsInChildren<Toggle> (true);//include inactive

	}

	void ResetLobby()
	{
		foreach (Toggle t in RolesToggleArray) {
			t.isOn = false;
			t.interactable = true;
		}
		RacesToggleArray [0].isOn = true;
	}

	public void SetRace(int i)
	{
		//PersistantManager.GetInstance().SetRole(i);
		if (Network.isServer)
			SetRaceAction (i, Network.player);
		else
			networkView.RPC ("SetRaceAction", RPCMode.Server, new object[]{i,Network.player});
	}

	public void SetRole(int i)
	{
		//PersistantManager.GetInstance().SetRole(i);
		bool interactable = !(RolesToggleArray [i - 1].isOn);
		networkView.RPC ("SetRoleAction", RPCMode.All, new object[]{i,interactable,Network.player});

	}

	//incase a player drops need to unselect his role if he selected one
	public void PlayerDroppedOut(NetworkPlayer player)
	{
	 	PersistantManager.PlayerInfo pi = PersistantManager.GetInstance ().GetPlayerInfo (player);
		if (pi.Role != PersistantManager.Roles.RoleUnknown) {
			int role = (int)pi.Role;
			RolesToggleArray [role - 1].interactable = true;
			networkView.RPC ("SetRoleAction", RPCMode.Others, new object[]{role,true,Network.player});

		}
	}

	public void SetupLobby(bool isServer)
	{
		ResetLobby ();
		if (isServer) {
			ActionButtonText.text = StartStr;
		} else {
			ActionButtonText.text = ReadyStr;
			PlayerIsReady = false;
			networkView.RPC ("RequestLobbyInfo", RPCMode.Server, null);

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
				networkView.RPC("StartGame",RPCMode.All,null);
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
	void StartGame()
	{
		Application.LoadLevel (1);
	}

	[RPC]
	void RequestLobbyInfo(NetworkMessageInfo info)
	{
		int index = 1;
		foreach (Toggle t in RolesToggleArray) {
			bool isSelectable = (t.interactable && !t.isOn);
			if(isSelectable == false)//only send info on the ones that are already selected
				networkView.RPC ("SetRoleAction", info.sender, new object[]{index,isSelectable,Network.player});
			index++;
		}
	}

	[RPC]
	void SetPlayerReady(bool isReady,NetworkMessageInfo info)
	{
		PersistantManager.PlayerInfo p = PersistantManager.GetInstance().GetPlayerInfo(info.sender);
		p.IsReady = !p.IsReady;
	}

	[RPC]
	void SetRoleAction(int role, bool interactable, NetworkPlayer player)
	{
		if (Network.isServer) {
			PersistantManager.PlayerInfo p = PersistantManager.GetInstance ().GetPlayerInfo (player);
			if (p.Role == PersistantManager.Roles.RoleUnknown + role)
				p.Role = PersistantManager.Roles.RoleUnknown;
			else
				p.Role = PersistantManager.Roles.RoleUnknown + role;


		}

		if (player == Network.player)
			return;//skip if self

		RolesToggleArray [role - 1].interactable = interactable;

	}

	[RPC]
	void SetRaceAction(int race, NetworkPlayer player)
	{
		PersistantManager.PlayerInfo p = PersistantManager.GetInstance ().GetPlayerInfo (player);
		p.Race = PersistantManager.Races.RaceUnknown + race;
	}
}
