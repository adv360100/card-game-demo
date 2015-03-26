using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class PlayersPanel : MonoBehaviour {

	public Transform Panel;
	public GameObject TextObject;

	private List<string> PlayerNameList = new List<string>();

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	[RPC]
	public void AddPlayer(string name)
	{
		if (PlayerNameList.Contains (name))
			return;//skip as it is already in the list//Note: need to test 

		GameObject player = Instantiate (TextObject) as GameObject;
		Text t = player.GetComponent<Text> ();
		t.text = name;
		player.transform.SetParent(Panel);
		PlayerNameList.Add (name);
	}

	[RPC]
	public string[] GetPlayerList()
	{
		return PlayerNameList.ToArray ();
	}
}
