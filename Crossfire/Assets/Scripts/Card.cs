using UnityEngine;
using System.Collections;

public enum CardLocation {CardLocationDeck = 0, CardLocationDiscard, CardLocationCurrentPlayer, CardLocationOtherPlayer, 
							CardLocationMarketDeck, CardLocationMarketField, CardLocationCrossfireDeck};

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
		GameObject.FindGameObjectWithTag ("MainCamera").GetComponent<MoveCamera> ().ZoomOnObject (renderer);
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
