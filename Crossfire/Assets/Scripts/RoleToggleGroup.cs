using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class RoleToggleGroup : MonoBehaviour {

	private Toggle[] ToggleArray;

	// Use this for initialization
	void Start () {
		ToggleArray = GetComponentsInChildren<Toggle> (true);//include inactive
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void SetRole(int i)
	{
		PersistantManager.GetInstance().SetRole(i);
		networkView.RPC ("UpdateGroup", RPCMode.AllBuffered, ToggleArray);

	}

	[RPC]
	public void UpdateGroup(Toggle[] array)
	{

	}
}
