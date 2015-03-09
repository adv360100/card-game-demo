using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CrossfireDeck : Deck {

	public BasicArea AreaManager;

	void OnMouseDown()
	{
		if (CardList.Count <= 0) {
			return;
		}
		
		GameObject topCard = CardList [CardList.Count - 1];
		topCard.GetComponent<Card>().CurrentCardLocation = CardLocation.CardLocationCrossfireDeck;
		topCard.GetComponent<Renderer>().enabled = true;
		CardList.Remove (topCard);

		topCard.GetComponent<Card>().QuadraticOutMoveTo(topCard.transform.position,
		                                                AreaManager.DiscardPile.transform.position,
		                                                1f,
		                                                ()=>{
			AreaManager.DiscardPile.AddCard(topCard);
			topCard.transform.parent = AreaManager.DiscardPile.transform;											
		});
	}
}
