using UnityEngine;
using System.Collections;

public class Obstacle : MonoBehaviour {

	public uint ID;
	public string ObstacleName;

	// Use this for initialization
	void Start () {
		ID = (uint)Random.Range (0, 100);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
