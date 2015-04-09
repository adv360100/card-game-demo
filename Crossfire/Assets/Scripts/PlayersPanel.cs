using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayersPanel : MonoBehaviour {

	public Transform Panel;
	public GameObject TextObject;


	[RPC]
	public void AddPlayerName(string name)
	{
		GameObject player = Instantiate (TextObject) as GameObject;
		Text t = player.GetComponent<Text> ();
		t.text = name;
		player.transform.SetParent(Panel);
	}

	public void ClearNames()
	{
		Text[] names = Panel.GetComponentsInChildren<Text> ();
		foreach (Text comp in names) {
			Destroy(comp.gameObject);
		}
	}
}
