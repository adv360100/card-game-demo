using UnityEngine;
using System.Collections;

public class MarketDeck : Deck {

	public BlackMarketArea MarketArea;

	override public GameObject DrawCard()
	{
		if (CardList.Count <= 0)
		{
			if(MarketArea.GetDiscardPile().Count <= 0)
			{
				return null;
			}
			//shuffle discard into deck and try draw again
			AddCards(MarketArea.GetDiscardPile());
			ShuffleDeck(CardList.ToArray());
			MarketArea.RemoveDiscardPile();
		}
		
		GameObject topCard = CardList [CardList.Count - 1];
		topCard.GetComponent<Card>().CurrentCardLocation = CardLocation.CardLocationCurrentPlayer;
		CardList.Remove (topCard);
		return topCard;
	}

}
