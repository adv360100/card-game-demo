using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BlackMarketArea : BasicArea {
	
	public List<GameObject> CardList; //cards in the market area
	public int Columns = 3;
	public int CardMax = 6;
	public float CardSpacing = 2f;

	void Update()
	{
		bool needsUpdate = false;
		while (CardList.Count < CardMax)
		{
			GameObject card = MainDeck.DrawCard();
			if(card == null)
				break;
			needsUpdate = true;
			card.transform.position = MainDeck.transform.position;
			card.transform.parent = transform;
			CardList.Add(card);
		}

		if (needsUpdate)
			UpdateDeckDisplay (1f);
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

	void UpdateDeckDisplay(float animationDuration)
	{
		if (CardList.Count == 0)
			return;

		float objectWidth = CardList [0].renderer.bounds.size.x;
		float objectHeight = CardList [0].renderer.bounds.size.y;
		int index = 0;
		//find start point
		float startPointX = MainDeck.transform.position.x + objectWidth + CardSpacing;
		float startPointY = MainDeck.transform.position.y;
		foreach (GameObject cardObject in CardList) 
		{
			Card card = cardObject.GetComponent<Card>();
			card.GetComponent<Renderer>().enabled = true;
			float x = startPointX + (objectWidth + CardSpacing) * (index % Columns);
			int currentRow = index / Columns;
			float y = startPointY - (objectHeight + CardSpacing) * currentRow;//subtract so that the cards go down the y axis
			card.QuadraticOutMoveTo (card.transform.position, new Vector3(x, y, MainDeck.transform.position.z), animationDuration, () => { });
			index++;
		}
	}
}
