using UnityEngine;
using System.Collections;

public enum CardLocation {CardLocationDeck = 0, CardLocationDiscard, CardLocationCurrentPlayer, CardLocationOtherPlayer, 
							CardLocationMarketDeck, CardLocationMarketField};

public class Card : BasicAnimator {

	public CardLocation CurrentCardLocation;
	public PlayerActions PlayerManager;

	// Use this for initialization
	void Start () {
	}

	void Awake () {
		CurrentCardLocation = CardLocation.CardLocationDeck;
	}

	void OnMouseDown () {
		if (CurrentCardLocation == CardLocation.CardLocationCurrentPlayer) {
			// Move to discard pile
			PlayerManager.MoveCardToDiscardFromHand(this);
			CurrentCardLocation = CardLocation.CardLocationDiscard;
		}
	}
}
