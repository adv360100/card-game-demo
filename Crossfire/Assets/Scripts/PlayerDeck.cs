using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerDeck : Deck {

	public PlayerArea PlayerManager;

	public GameObject[] DrawCards (int drawNumber) {
		GameObject card;
		
		GameObject[] drawnCards = new GameObject[drawNumber];
		for (int i = 0; i < drawNumber; i++) {
			card = DrawCard ();
			
			if (null == card) {
				//no more cards to draw
				break;
			}
			
			drawnCards[i] = card;
		}
		
		return drawnCards;
	}

	override public void OnMouseDown () {
		GameManager.Instance.networkView.RPC ("DrawPlayerCard", RPCMode.All, null);
	}

	[RPC]
	void DrawPlayerCard()
	{
		GameObject[] cards = DrawCards (1);
		foreach (GameObject cardObject in cards) {
			if (cardObject == null) {
				continue;
			}
			cardObject.transform.position = transform.position;
			PlayerManager.AddCardToHand (cardObject);
		}
		
		UpdateDeckDisplay ();
	}
}
