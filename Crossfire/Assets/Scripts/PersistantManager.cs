using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PersistantManager : MonoBehaviour {

	public class PlayerInfo {
		public string Name;
		public NetworkPlayer Player;
		public Races Race;
		public Roles Role;
		public bool IsReady;
	}

	static private PersistantManager Instance = null;

	public enum Races {RaceUnknown = 0, RaceHuman, RaceElf, RaceDwarf, RaceOrk, RaceTroll}; 
	public enum Roles {RoleUnknown = 0, RoleStreetSamurai, RoleMage, RoleDecker, RoleFace}; 
	[HideInInspector]public Races SelectedRace = Races.RaceUnknown + 1;
	[HideInInspector]public Roles SelectedRole = Roles.RoleUnknown + 1;
	[HideInInspector]public List<PlayerInfo> Players = new List<PlayerInfo> ();

	// Use this for initialization
	void Awake () {
		if (Instance != null) {
			DestroyObject(gameObject);
			return;
		}

		Instance = this;
		DontDestroyOnLoad (gameObject);
	}

	static public PersistantManager GetInstance()
	{
		if(Instance == null)
		{
			GameObject obj = new GameObject("Managers");
			obj.AddComponent<PersistantManager>();
			//Instance should be updated in awake
		}

		return Instance;
	}

	public void SetRole(int i)
	{
		SelectedRole = Roles.RoleUnknown + i ;
	}

	public void SetRace(int i)
	{
		SelectedRace = Races.RaceUnknown + i;
	}

	[RPC]
	public void AddPlayer(NetworkPlayer player, string name)
	{
		PlayerInfo p = new PlayerInfo ();
		p.Name = name;
		p.Player = player;
		p.Race = Races.RaceUnknown;
		p.Role = Roles.RoleUnknown;
		p.IsReady = false;
		Players.Add (p);
	}

	[RPC]
	public void RemovePlayer(NetworkPlayer player)
	{
		int index = -1;
		foreach (PlayerInfo p in Players) {
			index++;
			if(p.Player == player)
			{
				break;
			}
		}

		if (index >= 0) {
			Players.RemoveAt(index);
		}
	}


	public PlayerInfo GetPlayerInfo(NetworkPlayer player)
	{
		int index = -1;
		foreach (PlayerInfo p in Players) {
			index++;
			if(p.Player == player)
			{
				break;
			}
		}
		
		if (index >= 0) {
			return Players[index];
		}

		return new PlayerInfo();
	}

}
