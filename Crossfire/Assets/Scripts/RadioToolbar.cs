using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class RadioToolbar : MonoBehaviour {

	public Button SelectedButton;
	public Button[] PlayerButtons;

	// Use this for initialization
	void Start () {
		SelectedButton.gameObject.SetActive (false);

		List<PersistantManager.PlayerInfo> players = PersistantManager.GetInstance ().Players;
		for (int i = 0; i < PlayerButtons.Length; i++) {
			if (i < players.Count) {
				if (players[i] != PersistantManager.GetInstance ().GetPlayerInfo (Network.player)) {
					PlayerButtons[i].GetComponentInChildren<Text> ().text = players[i].Name;
				}
			} else {
				PlayerButtons[i].gameObject.SetActive (false);
			}
		}


	}

	public void SelectButton (Button btn) {
		SelectedButton.gameObject.SetActive (true);
		SelectedButton = btn;
		btn.gameObject.SetActive (false);
	}
}
