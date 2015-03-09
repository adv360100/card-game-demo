using UnityEngine;
using System.Collections;

public class ObstacleDeck : Deck {

	public GameManager MyGameManager;

	void OnMouseDown()
	{
		GameObject card = DrawCard ();
		if (card == null) {
			return;
		}

		card.transform.position = transform.position;
		MyGameManager.AddObstacleToMyPlayer (card);
		
		UpdateDeckDisplay ();
	}
}
