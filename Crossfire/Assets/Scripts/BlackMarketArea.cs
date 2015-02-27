using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BlackMarketArea : BasicArea {

	public Deck MarketDeck;
	public DiscardDeck DiscardDeck;
	public GameObject BasicCard;
	public List<GameObject> CardList; //cards in the market area
	public int Columns = 3;
	public int Rows = 2;
	public int CardMax = 6;

	// Use this for initialization
	void Start () {
		if (MarketDeck != null)
		{
			List<GameObject> cardList = new List<GameObject>();
			for (int i = 0; i < 10; i++) {
				GameObject cardToAdd = GameObject.Instantiate(BasicCard, MarketDeck.transform.position, MarketDeck.transform.rotation) as GameObject;
				
				Vector3 pos = cardToAdd.transform.position;
				pos.z = float.MaxValue;
				cardToAdd.transform.position = pos;
				cardList.Add(cardToAdd);
			}
			
			MarketDeck.AddCards(cardList);
		}
	}

	void Update()
	{
		bool needsUpdate = false;
		while (CardList.Count < CardMax)
		{
			GameObject card = MarketDeck.DrawCard();
			if(card == null)
				break;
			needsUpdate = true;
			card.transform.position = transform.position;
			CardList.Add(card);
		}

		if (needsUpdate)
			UpdateDeckDisplay (1f);
	}
	
	public List<GameObject> GetDiscardPile () {
		return DiscardDeck.CardList;
	}

	public void RemoveDiscardPile () {
		Vector3 curPos = DiscardDeck.transform.position;
		DiscardDeck.AnimateTo(MarketDeck.transform.position, 1.0f, p => {
			DiscardDeck.RemoveAllCards();
			DiscardDeck.UpdateDeckDisplay();
			DiscardDeck.transform.position = curPos;
			return 0;
		});
	}

	override public void OnFocusEnter()
	{
		
	}

	override public void OnFocusExit()
	{
		
	}

	public void AnimateTarget(GameObject target, float rate) {
		CardList.Add (target);
		UpdateDeckDisplay (rate);
	}

	void UpdateDeckDisplay(float animationRate)
	{
		float objectWidth = CardList [0].renderer.bounds.size.x;
		float objectHeight = CardList [0].renderer.bounds.size.y;
		int count = Columns - 1;
		float containerWidth = count * objectWidth;
		count = Rows - 1;
		float containerHeight = Rows * objectHeight;
		int index = 0;
		//find start point
		float startPointX = containerWidth * -0.5f;
		float startPointY = containerHeight * -0.5f;
		foreach (GameObject cardObject in CardList) 
		{
			Card card = cardObject.GetComponent<Card>();
			float x = startPointX + objectWidth * (index / Rows);
			float y = startPointY + objectHeight * (index / Columns);
			card.AnimateTo(new Vector3(x, y, transform.position.z),animationRate);
			index++;
		}
	}
}
