using UnityEngine;
using System.Collections;

public class ObstacleDeck : Deck {
	
	override public void OnMouseDown () {
		GameManager.Instance.networkView.RPC ("DrawObstacle", RPCMode.All, new object[]{GameManager.Instance.MyPlayer.networkView.viewID});
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
		card.GetComponent<Card> ().AreaManager = GameManager.Instance.GetPlayerAreaForNetworkViewID(viewID);
		GameManager.Instance.AddObstacleToMyPlayer (card);

		UpdateDeckDisplay ();
		ObstacleActions.Instance.CheckObstacleButtons ();
	}
}
