﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DeckSetup : MonoBehaviour {

	public DeckActions MainDeck;
	public GameObject OriginalCard;

	// Use this for initialization
	void Start () {
		// Create the starting deck
		if (MainDeck != null) { 
			List<GameObject> cardList = new List<GameObject>();
			for (int i = 0; i < 10; i++) {
				GameObject cardToAdd = GameObject.Instantiate(OriginalCard, MainDeck.transform.position, MainDeck.transform.rotation) as GameObject;

				Vector3 pos = cardToAdd.transform.position;
				pos.z = 1;
				cardToAdd.transform.position = pos;
				cardList.Add(cardToAdd);
			}

			MainDeck.AddCards(cardList);
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}