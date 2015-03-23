using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayersPanel : MonoBehaviour {

	public Transform Panel;
	public GameObject TextObject;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	[RPC]
	public void AddPlayer(string name)
	{
		GameObject player = Instantiate (TextObject) as GameObject;
		Text t = player.GetComponent<Text> ();
		t.text = name;
		player.transform.SetParent(Panel);
		//VerticalLayoutGroup g = Panel.GetComponent<VerticalLayoutGroup> ();

	}
}
