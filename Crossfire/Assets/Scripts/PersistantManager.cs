using UnityEngine;
using System.Collections;

public class PersistantManager : MonoBehaviour {

	static private PersistantManager Instance = null;

	public enum Races {RaceUnknown = 0, RaceHuman, RaceElf, RaceDwarf, RaceOrk, RaceTroll}; 
	public enum Roles {RoleUnknown = 0, RoleStreetSamurai, RoleMage, RoleDecker, RoleFace}; 
	[HideInInspector]public Races SelectedRace = Races.RaceUnknown + 1;
	[HideInInspector]public Roles SelectedRole = Roles.RoleUnknown + 1;

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
}
