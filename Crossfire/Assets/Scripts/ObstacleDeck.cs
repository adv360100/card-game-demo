using UnityEngine;
using System.Collections;

public class ObstacleDeck : Deck {
	
	override public void OnMouseDown () {
		GameObject card = DrawCard ();
		if (card == null) {
			return;
		}
		
		card.transform.position = transform.position;
		card.tag = ObstacleActions.kObstacleTag;
		card.GetComponent<Card> ().AreaManager = GameManager.Instance.MyPlayer;
		GameManager.Instance.AddObstacleToMyPlayer (card);
		
		UpdateDeckDisplay ();
		ObstacleActions.Instance.CheckObstacleButtons ();
	}
}
