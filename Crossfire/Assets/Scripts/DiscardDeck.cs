using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class DiscardDeck : Deck {
	
	float AnimateRate;

	void Awake () {
		mainMesh = GetComponent<MeshRenderer> ();
		UpdateDeckDisplay ();
	}




	public void AddCard (GameObject card) {
		CardList.Add (card);
		UpdateDeckDisplay ();
	}

	public void RemoveAllCards () {
		CardList.Clear ();
		UpdateDeckDisplay ();
	}

	public bool ContainsCard (Card card) {
		return CardList.Contains (card.gameObject);
	}
}
