using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerActions : MonoBehaviour {

	public Deck MainDeck;
	public DiscardDeck DiscardPile;
	public HandActions Hand;
	public GameObject PlayField;
	public GameObject OriginalCard;

	// Use this for initialization
	void Start () {
		// Create the starting deck
		if (MainDeck != null) { 
			List<GameObject> cardList = new List<GameObject>();
			for (int i = 0; i < 5; i++) {
				GameObject cardToAdd = GameObject.Instantiate(OriginalCard, MainDeck.transform.position, MainDeck.transform.rotation) as GameObject;
				cardToAdd.GetComponent<Card>().CurrentCardLocation = CardLocation.CardLocationDeck;
				cardToAdd.GetComponent<Card>().PlayerManager = this;

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

	public List<GameObject> GetDiscardPile () {
		return DiscardPile.CardList;
	}

	public void RemoveDiscardPile () {
		Vector3 curPos = DiscardPile.transform.position;
		DiscardPile.QuadraticOutMoveTo (DiscardPile.transform.position, MainDeck.transform.position, 1.0f, () => {
			DiscardPile.RemoveAllCards ();
			DiscardPile.UpdateDeckDisplay ();
			DiscardPile.transform.position = curPos;
		});
	}
	
	public void MoveCardToDiscardFromHand (Card card) {
		if (DiscardPile.ContainsCard (card)) {
			return;
		}

		card.CurrentCardLocation = CardLocation.CardLocationDiscard;
		card.QuadraticOutMoveTo (card.transform.position, DiscardPile.transform.position, 1.0f, () => {
			DiscardPile.AddCard (card.gameObject);
			Hand.RemoveCard (card.gameObject);
		});
	}

	public void AddCardToHand (GameObject card) {
		Hand.AddCard (card);
	}

	public void EndTurn () {

	}
}
