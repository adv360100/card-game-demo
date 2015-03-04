using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObstacleActions : MonoBehaviour {

	const string kObstacleTag = "Obstacle";

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

			bool found = false;
			for (int j = 0; j < ButtonList.Count; j++) {
				if (ButtonList[j].GetComponent<ObstacleButton> ().id == obstacles[i].GetComponent<Obstacle> ().id) {
					found = true;
					break;
				}
			}
			
			// If we already have this in our list, don't create it again
			if (found) {
				continue;
			}
			
			
		}
	}
}
