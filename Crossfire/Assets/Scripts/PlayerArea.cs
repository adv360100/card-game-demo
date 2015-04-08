using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerArea : BasicArea {
	
	public HandActions Hand;
	public GameObject PlayField;
	public HandActions ObstacleSection;

	//MoveCardToDiscardFromHand
	override public void MoveCard (Card card) {
		if (card.tag == ObstacleActions.kObstacleTag) {
			networkView.RPC("RemoveObstacle",RPCMode.All,new object[]{(int)card.ID});
		} else {
			if (Hand.CardList.Contains (card.gameObject) == false ) {
				card.AttachedObstacle.RemoveCard(card);
				AddCardToHand(card.gameObject);
				return;
			}

			AddCardToObstacle(card);
		}
	}

	[RPC]
	void RemoveObstacle(int cardID)
	{
		Card card = null;
		foreach (GameObject obj in ObstacleSection.CardList) {
			Card temp = obj.GetComponent<Card>();
			if(temp.ID == cardID)
			{
				card = temp;
				break;
			}
		}
		if (card == null)
			return;

		ObstacleSection.RemoveCard (card.gameObject);
		Obstacle ob = card as Obstacle;
		ob.CardList.ForEach(delegate(Card obj) {
			//animate player cards to discard
			obj.QuadraticOutMoveTo (obj.transform.position, DiscardPile.transform.position, 1.0f, () => {
				DiscardPile.AddCard (obj.gameObject);
				obj.GetComponent<Renderer>().enabled = false;
			});
		});
		
		
		ob.RemoveAllCards();
		GameManager.Instance.DiscardObstacle (card.gameObject);
	}
	public void AddCardToHand (GameObject card) {
		Hand.AddCard (card);
	}

	public void ShowObstacleTargetsUI () {
		
	}

	override public void OnFocusEnter () {
		
	}

	override public void OnFocusExit () {
		
	}

	void AddCardToObstacle(Card c)
	{
		uint i = GameManager.Instance.ObstaclePanel.selectedObstacleID;
		if(i > 0)
		{
			foreach(GameObject gameobject in ObstacleSection.CardList)
			{
				Obstacle ob = gameobject.GetComponent<Obstacle>();
				if(ob.ID == i)
				{
					Hand.RemoveCard(c.gameObject);
					ob.PlayCard(c);
					break;
				}
			}
	    }
	}
}
