using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerActions : MonoBehaviour {

	public DeckAnimator MainDeck;
	public DiscardActions DiscardPile;
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
		DiscardPile.AnimateTo(MainDeck.transform.position, 1.0f, p => {
			DiscardPile.RemoveAllCards();
			DiscardPile.UpdateDisplay();
			DiscardPile.transform.position = curPos;
			return 0;
		});
	}
	
	public void MoveCardToDiscardFromHand (Card card) {
		DiscardPile.AddCard (card.gameObject);
		Hand.RemoveCard (card.gameObject);
		card.AnimateTo (DiscardPile.transform.position, 1.0f);
	}

	public void AddCardToHand (GameObject card) {
		Hand.AnimateTarget (card, 1.0f);
	}

	public void EndTurn () {

	}
}
