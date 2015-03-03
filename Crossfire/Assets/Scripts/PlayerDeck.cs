using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerDeck : Deck {

	public PlayerActions PlayerManager;

	override public GameObject DrawCard() {
		if (CardList.Count <= 0) {
			if (PlayerManager.GetDiscardPile().Count <= 0) {
				return null;
			}
			//shuffle discard into deck and try draw again
			ShuffleDeck (CardList.ToArray ());
			List<GameObject> discardPile = new List<GameObject> (PlayerManager.GetDiscardPile ());
			PlayerManager.RemoveDiscardPile (() => {
				AddCards (discardPile);
				OnMouseDown ();
			});
			return null;
		}
		
		GameObject topCard = CardList [CardList.Count - 1];
		topCard.GetComponent<Card>().CurrentCardLocation = CardLocation.CardLocationCurrentPlayer;
		topCard.GetComponent<Renderer>().enabled = true;
		CardList.Remove (topCard);
		return topCard;
	}

	public GameObject[] DrawCards(int drawNumber)
	{
		GameObject card;
		
		GameObject[] drawnCards = new GameObject[drawNumber];
		for (int i=0; i < drawNumber; i++) {
			card = DrawCard();
			
			if(null == card)
			{
				//no more cards to draw
				break;
			}
			
			drawnCards[i] = card;
		}
		
		return drawnCards;
	}

	void OnMouseDown()
	{
		GameObject[] cards = DrawCards (1);
		foreach (GameObject cardObject in cards) {
			if (cardObject == null) {
				continue;
			}
			cardObject.transform.position = transform.position;
			PlayerManager.AddCardToHand(cardObject);
		}
		
		UpdateDeckDisplay ();
	}
}
