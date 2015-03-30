using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class BlackMarketArea : BasicArea {
	
	public List<GameObject> CardList; //cards in the market area
	public int Columns = 3;
	public int CardMax = 6;
	public float CardSpacing = 2f;

	void Update () {
		if (!GameManager.Instance.IsSyncing) {
			bool needsUpdate = false;
			while (CardList.Count < CardMax) {
				GameObject card = MainDeck.DrawCard ();
				if (card == null)
					break;
				needsUpdate = true;
				card.transform.position = MainDeck.transform.position;
				card.transform.parent = transform;
				CardList.Add (card);
			}

			if (needsUpdate)
				UpdateDeckDisplay (1f);
		}
	}

	override public void OnFocusEnter () {
		
	}

	override public void OnFocusExit () {
		
	}

	override public void MoveCard (Card card) {
		//move card to current player hand
		BasicArea playerArea = GameManager.GetCurrentPlayerArea();

		if (playerArea != GameManager.Instance.MyPlayer) {
			card.GetComponent<Renderer> ().material.SetTexture (0, MainDeck.GetComponent<Renderer> ().material.GetTexture(0));
		}

		card.AreaManager = playerArea;
		CardList.Remove (card.gameObject);
		card.gameObject.transform.parent = playerArea.gameObject.transform;
		playerArea.gameObject.GetComponent<PlayerArea> ().AddCardToHand (card.gameObject);
	}

	void UpdateDeckDisplay(float animationDuration) {
		if (CardList.Count == 0)
			return;

		float objectWidth = CardList [0].renderer.bounds.size.x;
		float objectHeight = CardList [0].renderer.bounds.size.y;
		int index = 0;
		//find start point
		float startPointX = MainDeck.transform.position.x + objectWidth + CardSpacing;
		float startPointY = MainDeck.transform.position.y;
		foreach (GameObject cardObject in CardList) {
			Card card = cardObject.GetComponent<Card>();
			card.GetComponent<Renderer>().enabled = true;
			float x = startPointX + (objectWidth + CardSpacing) * (index % Columns);
			int currentRow = index / Columns;
			float y = startPointY - (objectHeight + CardSpacing) * currentRow;//subtract so that the cards go down the y axis
			card.QuadraticOutMoveTo (card.transform.position, new Vector3 (x, y, MainDeck.transform.position.z), animationDuration, null);
			index++;
		}
	}

	public List<GameObject> PullPlayerDeck (PersistantManager.Roles role, BasicArea newManager) {
		// A Player deck starts as 1 basic card of each and then 3 of your role
		List<GameObject> playerCards = new List<GameObject> (7);

		playerCards.Add (PullPlayerCard ("Mark", newManager));
		playerCards.Add (PullPlayerCard ("Mana", newManager));
		playerCards.Add (PullPlayerCard ("Street Smarts", newManager));
		playerCards.Add (PullPlayerCard ("Quick Shot", newManager));

		string mainRole = "";
		switch (role) {
		case PersistantManager.Roles.RoleDecker:
			mainRole = "Mark";
			break;
		case PersistantManager.Roles.RoleFace:
			mainRole = "Street Smarts";
			break;
		case PersistantManager.Roles.RoleMage:
			mainRole = "Mana";
			break;
		case PersistantManager.Roles.RoleStreetSamurai:
		default:
			mainRole = "Quick Shot";
			break;
		}

		for (int i = 0; i < 3; i++) {
			playerCards.Add (PullPlayerCard (mainRole, newManager));
		}

		return playerCards;
	}

	GameObject PullPlayerCard (string cardName, BasicArea newManager) {
		GameObject temp = MainDeck.CardList.Find (delegate(GameObject obj) {
			return obj.name == cardName;
		});

		Card card = temp.GetComponent<Card> ();

		if (newManager != GameManager.Instance.MyPlayer) {
			card.GetComponent<Renderer> ().material.SetTexture (0, MainDeck.GetComponent<Renderer> ().material.GetTexture(0));
		}
		
		card.AreaManager = newManager;
		card.gameObject.transform.parent = newManager.gameObject.transform;

		MainDeck.CardList.Remove (temp);

		return temp;
	}
}
