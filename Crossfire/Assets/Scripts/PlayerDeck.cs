using UnityEngine;
using System.Collections;

public class PlayerDeck : Deck {

	public PlayerActions PlayerManager;

	override public GameObject DrawCard()
	{
		if (CardList.Count <= 0)
		{
			if(PlayerManager.GetDiscardPile().Count <= 0)
			{
				return null;
			}
			//shuffle discard into deck and try draw again
			AddCards(PlayerManager.GetDiscardPile());
			ShuffleDeck(CardList.ToArray());
			PlayerManager.RemoveDiscardPile();
		}
		
		GameObject topCard = CardList [CardList.Count - 1];
		topCard.GetComponent<Card>().CurrentCardLocation = CardLocation.CardLocationCurrentPlayer;
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
		if (DeckCount () == 0)
			return;
		GameObject[] cards = DrawCards (1);
		foreach (GameObject cardObject in cards) 
		{
			cardObject.transform.position = transform.position;
			PlayerManager.AddCardToHand(cardObject);
		}
		
		UpdateDeckDisplay ();
	}
}
