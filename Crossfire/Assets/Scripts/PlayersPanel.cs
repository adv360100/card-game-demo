using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class PlayersPanel : MonoBehaviour {

	public Transform Panel;
	public GameObject TextObject;

	private List<string> PlayerNameList = new List<string>();

	[RPC]
	public void AddPlayerName(string name)
	{
		//if (PlayerNameList.Contains (name))
		//	return;//skip as it is already in the list//Note: need to test 

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
