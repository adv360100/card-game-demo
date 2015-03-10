using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObstacleArea : BasicArea {
	public Deck HardDeck;

	override public void Start () {
		base.Start ();
		if (HardDeck != null) {
			List<GameObject> cardList = new List<GameObject>();
			for (int i = 0; i < 10; i++) {
				Vector3 newPos = new Vector3(MainDeck.transform.position.x, MainDeck.transform.position.y, MainDeck.transform.position.z + 1);
				GameObject cardToAdd = GameObject.Instantiate(BasicCard, newPos, MainDeck.transform.rotation) as GameObject;
				//parent card for easier debugging
				cardToAdd.transform.parent = MainDeck.transform;
				cardToAdd.GetComponent<Card>().AreaManager = this;
				cardToAdd.GetComponent<Renderer>().enabled = false;
				cardList.Add(cardToAdd);
			}
			
			HardDeck.AddCards(cardList);
			HardDeck.UpdateDeckDisplay();
		}
	}
}
