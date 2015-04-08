using UnityEngine;
using System.Collections;

public class ObstacleDeck : Deck {
	
	override public void OnMouseDown () {
		networkView.RPC ("DrawObstacle", RPCMode.All, new object[]{GameManager.Instance.MyPlayer.networkView.viewID});
	}

	[RPC]
	void DrawObstacle(NetworkViewID viewID)
	{
		GameObject card = DrawCard ();
		if (card == null) {
			return;
		}
		
		card.transform.position = transform.position;
		card.tag = ObstacleActions.kObstacleTag;
		PlayerArea actingPlayerArea = GameManager.Instance.GetPlayerAreaForNetworkViewID (viewID);
		card.GetComponent<Card> ().AreaManager = actingPlayerArea;
		actingPlayerArea.ObstacleSection.AddCard (card);

		UpdateDeckDisplay ();
		ObstacleActions.Instance.CheckObstacleButtons ();
	}

	override public GameObject DrawCard () {
		if (CardList.Count <= 0) {
			return null;
		}
		
		GameObject topCard = CardList [CardList.Count - 1];
		topCard.GetComponent<Renderer>().enabled = true;
		CardList.Remove (topCard);
		return topCard;
	}

}
