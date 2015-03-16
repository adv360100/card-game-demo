using UnityEngine;
using System.Collections;

public class Card : BasicAnimator {

	public uint ID;
	public BasicArea AreaManager;
	public Texture FrontTexture;

	// Use this for initialization
	void Start () {
		//TODO: assign unique id
		ID = (uint)Random.Range (1, 100);
	}

	void Awake () {
	}

	void OnMouseOver () {
		if (Input.GetMouseButtonDown (0)) { // Left click
			AreaManager.MoveCard(this);
		} else if (Input.GetMouseButtonDown (1)) { // Right click
			GameObject.FindGameObjectWithTag ("MainCamera").GetComponent<MoveCamera> ().ZoomOnObject (renderer);
		}
	}
}
