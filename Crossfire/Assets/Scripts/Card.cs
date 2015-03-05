using UnityEngine;
using System.Collections;

public enum CardLocation {CardLocationDeck = 0, CardLocationDiscard, CardLocationCurrentPlayer, CardLocationOtherPlayer, 
							CardLocationMarketDeck, CardLocationMarketField};

public class Card : BasicAnimator {

	public CardLocation CurrentCardLocation;
	public BasicArea AreaManager;

	// Use this for initialization
	void Start () {
	}

	void Awake () {
		CurrentCardLocation = CardLocation.CardLocationDeck;
	}

	void OnMouseDown () {
//		if (CurrentCardLocation == CardLocation.CardLocationCurrentPlayer ||
//		    CurrentCardLocation == CardLocation.CardLocationMarketField) {
//			// Move to discard pile
//			AreaManager.MoveCard(this);
//		}
	}

	override protected void OnDoubleClick()
	{
		if (CurrentCardLocation == CardLocation.CardLocationCurrentPlayer ||
		    CurrentCardLocation == CardLocation.CardLocationMarketField) {
			// Move to discard pile
			AreaManager.MoveCard(this);
		}
	}
}
