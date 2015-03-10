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
			ObstacleSection.RemoveCard (card.gameObject);
			GameManager.Instance.DiscardObstacle (card.gameObject);
		} else {
			if (DiscardPile.ContainsCard (card)) {
				return;
			}
			
			Hand.RemoveCard (card.gameObject);
			card.QuadraticOutMoveTo (card.transform.position, DiscardPile.transform.position, 1.0f, () => {
				DiscardPile.AddCard (card.gameObject);
				card.GetComponent<Renderer>().enabled = false;
			});
		}
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
}
