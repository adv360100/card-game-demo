using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class ObstacleActions : MonoBehaviour {

	static public ObstacleActions Instance;

	public static readonly string kObstacleTag = "Obstacle";
	static readonly int kButtonPadding = 10;
	static readonly Vector3 kFirstButtonPos = new Vector3(0, 90, 0);

	List<GameObject> ButtonList = new List<GameObject> (8);

	public GameObject OriginalObstacleButton;
	
	// Use this for initialization
	void Start () {
		if (Instance == null) {
			Instance = this;
		} else {
			DestroyObject(this);
			return;
		}
	}
	
	// Update is called once per frame
	void Update () {

	}

	public void UnselectAllObstacles () {
		foreach (GameObject button in ButtonList) {
			button.GetComponent<ObstacleButton> ().UnselectObstacle ();
		}
	}

	public void CheckObstacleButtons () {
		GameObject[] obstacles = GameObject.FindGameObjectsWithTag (kObstacleTag);

		RemoveAllButtons ();
		ButtonList.Clear ();
		if (obstacles.Length == 0) {
			return;
		}
		
		for (int i = 0; i < obstacles.Length; i++) {
			Card obstacle = obstacles[i].GetComponent<Card> ();
			
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

	void RemoveAllButtons () {
		foreach (GameObject button in ButtonList) {
			GameObject.DestroyImmediate(button);
		}
	}

	public void TintObstacle (uint ID) {
//		GameObject[] obstacles = GameObject.FindGameObjectsWithTag (kObstacleTag);
//		foreach (GameObject obj in obstacles) {
//			if (obj.GetComponent<Card> ().ID == ID) {
//				obj.GetComponent<Renderer> ().material.SetColor (0, new Color (0, 255, 0, 255));
//				return;
//			}
//		}
	}

	public void UntintObstacle (uint ID) {
//		GameObject[] obstacles = GameObject.FindGameObjectsWithTag (kObstacleTag);
//		foreach (GameObject button in obstacles) {
//			if (button.GetComponent<Card> ().ID == ID) {
//				button.GetComponent<Renderer> ().material.SetColor (0, new Color (1, 1, 1, 1));
//				return;
//			}
//		}
	}
}
