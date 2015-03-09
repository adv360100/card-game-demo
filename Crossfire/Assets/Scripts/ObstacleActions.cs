using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class ObstacleActions : MonoBehaviour {

	static readonly string kObstacleTag = "Obstacle";
	static readonly int kButtonPadding = 10;
	static readonly Vector3 kFirstButtonPos = new Vector3(0, 90, 0);

	public GameObject OriginalObstacleButton;

	private List<GameObject> ButtonList = new List<GameObject> ();

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		GameObject[] obstacles = GameObject.FindGameObjectsWithTag (kObstacleTag);
		if (obstacles.Length == 0) {
			return;
		}

		for (int i = 0; i < obstacles.Length; i++) {
			Card obstacle = obstacles[i].GetComponent<Card> ();

			bool found = false;
			for (int j = 0; j < ButtonList.Count; j++) {
				if (ButtonList[j].GetComponent<ObstacleButton> ().ID == obstacle.ID) {
					found = true;
					break;
				}
			}
			
			// If we already have this in our list, don't create it again
			if (found) {
				continue;
			}

			Vector3 buttonPos = Vector3.zero;
			if (ButtonList.Count > 0) {
				buttonPos = ButtonList[ButtonList.Count - 1].transform.localPosition;
				buttonPos.y -= ButtonList[ButtonList.Count - 1].GetComponent<RectTransform> ().sizeDelta.y + kButtonPadding;
			} else {
				buttonPos = kFirstButtonPos;
			}

			GameObject buttonToAdd = GameObject.Instantiate(OriginalObstacleButton, Vector3.zero, OriginalObstacleButton.transform.rotation) as GameObject;
			buttonToAdd.GetComponentInChildren<Text> ().text = obstacle.name;
			buttonToAdd.GetComponent<ObstacleButton> ().ID = obstacle.ID;
			buttonToAdd.GetComponent<ObstacleButton> ().ObstacleManager = this;
			buttonToAdd.transform.SetParent (transform);
			buttonToAdd.transform.localPosition = buttonPos;
			ButtonList.Add (buttonToAdd);
		}
	}

	public void UnselectAllObstacles () {
		foreach (GameObject button in ButtonList) {
			button.GetComponent<ObstacleButton> ().UnselectObstacle ();
		}
	}
}
