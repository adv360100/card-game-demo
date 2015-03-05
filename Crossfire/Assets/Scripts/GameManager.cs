using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public static BasicArea GetCurrentPlayerArea()
	{
		GameObject player = GameObject.FindGameObjectWithTag ("Player");
		//TODO: do some real stuff

		return player.GetComponent<BasicArea> ();
	}
}
