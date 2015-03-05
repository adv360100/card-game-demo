using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ObstacleButton : MonoBehaviour {

	public uint ID;
	public bool Selected;

	public ObstacleActions ObstacleManager;
	public Color SelectedColor;
	public Color UnselectedColor;

	// Use this for initialization
	void Start () {
		Selected = false;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void SelectObstacle () {
		ObstacleManager.UnselectAllObstacles ();
		GetComponent<Image> ().color = SelectedColor;
		Selected = true;
	}

	public void UnselectObstacle () {
		GetComponent<Image> ().color = UnselectedColor;
		Selected = false;
	}
}
