using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class DiscardDeck : Deck {

	public void AddCard (GameObject card) {
		CardList.Add (card);
		GetComponent<Renderer> ().material.SetTexture (0, card.GetComponent<Renderer> ().material.GetTexture(0));
		UpdateDeckDisplay ();
	}

	public void RemoveAllCards () {
		CardList.Clear ();
		UpdateDeckDisplay ();
	}

	public bool ContainsCard (Card card) {
		return CardList.Contains (card.gameObject);
	}

	void OnMouseOver () {
		if (Input.GetMouseButtonDown (1)) { // Right click
			GameObject.FindGameObjectWithTag ("MainCamera").GetComponent<MoveCamera> ().ZoomOnObject (renderer);
		}
	}
}
