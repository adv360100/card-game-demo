using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class RadioToolbar : MonoBehaviour {

	public Button SelectedButton;
	public Button[] PlayerButtons;

	// Use this for initialization
	void Start () {
		SelectedButton.gameObject.SetActive (false);

		for (int i = PersistantManager.GetInstance ().Players.Count; i < PlayerButtons.Length; i++) {
			PlayerButtons[i].gameObject.SetActive (false);
		}
	}

	public void SelectButton (Button btn) {
		SelectedButton.gameObject.SetActive (true);
		SelectedButton = btn;
		btn.gameObject.SetActive (false);
	}
}
