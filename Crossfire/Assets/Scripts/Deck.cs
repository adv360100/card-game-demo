using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//This class handles all the behind the scenes deck management
public class Deck : BasicAnimator {

	public List<GameObject> CardList = new List<GameObject>();
	public BasicArea Manager;
	public MeshRenderer[] ExtraCards; //the deck 'stack' cards for when there are more than one card in the deck

	public GameObject DrawCard () {
		if (CardList.Count <= 0) {
			if (Manager.GetDiscardPile().Count <= 0) {
				return null;
			}

			//shuffle discard into deck and try draw again
			ShuffleDeck (CardList.ToArray ());
			List<GameObject> discardPile = new List<GameObject> (Manager.GetDiscardPile ());
			Manager.RemoveDiscardPile (() => {
				AddCards (discardPile);
				OnMouseDown ();
			});
			return null;
		}
		
		GameObject topCard = CardList [CardList.Count - 1];
		topCard.GetComponent<Renderer>().enabled = true;
		CardList.Remove (topCard);
		return topCard;
	}

	virtual public void OnMouseDown () {
	}

	public int DeckCount () {
		return CardList.Count;
	}

	public void AddCards (IEnumerable<GameObject> cardsToAdd) {
		CardList.AddRange (cardsToAdd);
		UpdateDeckDisplay ();
	}

	protected void ShuffleDeck (GameObject[] deck) {
		for (int i = 0; i < deck.Length; i++) {
			GameObject temp = deck[i];
			int randomIndex = Random.Range(0, deck.Length);
			deck[i] = deck[randomIndex];
			deck[randomIndex] = temp;
		}
	}

	public void Shuffle () {
		GameObject[] deck = CardList.ToArray ();
		ShuffleDeck (deck);
		CardList.Clear ();
		foreach (GameObject c in deck) {
			CardList.Add(c);
		}
	}
	
	void Awake () {
		UpdateDeckDisplay ();
	}
	
	public void UpdateDeckDisplay () {
		int deckSize = DeckCount ();
		if (deckSize > 0)
			GetComponent<MeshRenderer> ().enabled = true;
		else
			GetComponent<MeshRenderer> ().enabled = false;
		
		for (int i = 0; i < ExtraCards.Length; i++) {
			MeshRenderer mesh = ExtraCards[i];
			if (deckSize > i + 1)
				mesh.enabled = true;
			else
				mesh.enabled = false;
			
		}
	}

}

public class DebugDeckActions : Deck {
	
	//get cards in deck for viewing 
	public GameObject[] GetDeck () {
		return CardList.ToArray();
	}
}

