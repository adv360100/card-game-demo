using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HandActions : MonoBehaviour {

	public List<GameObject> CardList = new List<GameObject>();

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void AddCards(IEnumerable<GameObject> cardsToAdd)
	{
		CardList.AddRange (cardsToAdd);
	}
}
