using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CrossfireDeck : Deck {
	
	override public void OnMouseDown()
	{
		GameManager.Instance.networkView.RPC ("DrawCrossfire", RPCMode.All, null);
	}

	[RPC]
	void DrawCrossfire()
	{
		if (CardList.Count <= 0) {
			return;
		}
		
		GameObject topCard = CardList [CardList.Count - 1];
		topCard.GetComponent<Renderer>().enabled = true;
		CardList.Remove (topCard);
		
		topCard.GetComponent<Card>().QuadraticOutMoveTo(topCard.transform.position,
		                                                Manager.DiscardPile.transform.position,
		                                                1f,
		                                                ()=>{
			Manager.DiscardPile.AddCard(topCard);
			topCard.transform.parent = Manager.DiscardPile.transform;											
		});
	}
}
