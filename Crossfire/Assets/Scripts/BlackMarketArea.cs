using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BlackMarketArea : BasicArea {

	public Deck MarketDeck;
	public Deck DiscardDeck;
	public GameObject BasicCard;
	public List<GameObject> CardList; //cards in the market area
	public int Columns = 3;
	public int Rows = 2;

	// Use this for initialization
	void Start () {
		if (MarketDeck != null)
		{
			List<GameObject> cardList = new List<GameObject>();
			for (int i = 0; i < 5; i++) {
				GameObject cardToAdd = GameObject.Instantiate(BasicCard, MarketDeck.transform.position, MarketDeck.transform.rotation) as GameObject;
				
				Vector3 pos = cardToAdd.transform.position;
				pos.z = 1;
				cardToAdd.transform.position = pos;
				cardList.Add(cardToAdd);
			}
			
			MarketDeck.AddCards(cardList);
		}
	}
	
	// Update is called once per frame
	void Update () {
	
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

	void UpdateDeckDisplay(float animationDuration)
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
			card.QuadraticOutMoveTo (card.transform.position, new Vector3(x, y, transform.position.z), animationDuration, () => { card.CurrentCardLocation = CardLocation.CardLocationCurrentPlayer; });
			index++;
		}
	}
}
