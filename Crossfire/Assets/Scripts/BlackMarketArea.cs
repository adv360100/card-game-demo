using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BlackMarketArea : BasicArea {

	public Deck MarketDeck;
	public DiscardDeck DiscardDeck;
	public GameObject BasicCard;
	public List<GameObject> CardList; //cards in the market area
	public int Columns = 3;
	public int CardMax = 6;
	public float CardSpacing = 2f;

	// Use this for initialization
	void Start () {
		if (MarketDeck != null)
		{
			List<GameObject> cardList = new List<GameObject>();
			for (int i = 0; i < 10; i++) {
				GameObject cardToAdd = GameObject.Instantiate(BasicCard, MarketDeck.transform.position, MarketDeck.transform.rotation) as GameObject;
				//parent card for easier debugging
				cardToAdd.transform.parent = MarketDeck.transform;
				cardToAdd.GetComponent<Card>().AreaManager = this;
				cardToAdd.GetComponent<Renderer>().enabled = false;
				//set card location
				Card cardComp = cardToAdd.GetComponent<Card>();
				cardComp.CurrentCardLocation = CardLocation.CardLocationMarketDeck;
				cardList.Add(cardToAdd);

			}
			
			MarketDeck.AddCards(cardList);
			MarketDeck.UpdateDeckDisplay();
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
			card.transform.position = MarketDeck.transform.position;
			card.transform.parent = transform;
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
		DiscardDeck.QuadraticOutMoveTo(DiscardDeck.transform.position, MarketDeck.transform.position, 1.0f, () => {
			DiscardDeck.RemoveAllCards();
			DiscardDeck.UpdateDeckDisplay();
			DiscardDeck.transform.position = curPos;
		});
	}

	override public void OnFocusEnter()
	{
		
	}

	override public void OnFocusExit()
	{
		
	}

	override public void MoveCard (Card card) {
		//move card to player hand
		GameObject hand = GameObject.FindGameObjectWithTag ("Player");
		CardList.Remove (card.gameObject);
		card.gameObject.transform.parent = hand.transform;
		hand.GetComponent<HandActions> ().AddCard (card.gameObject);
	}

	public void AnimateTarget(GameObject target, float rate) {
		CardList.Add (target);
		UpdateDeckDisplay (rate);
	}

	void UpdateDeckDisplay(float animationDuration)
	{
		if (CardList.Count == 0)
			return;

		float objectWidth = CardList [0].renderer.bounds.size.x;
		float objectHeight = CardList [0].renderer.bounds.size.y;
		int index = 0;
		//find start point
		float startPointX = MarketDeck.transform.position.x + objectWidth + CardSpacing;
		float startPointY = MarketDeck.transform.position.y;
		foreach (GameObject cardObject in CardList) 
		{
			Card card = cardObject.GetComponent<Card>();
			card.GetComponent<Renderer>().enabled = true;
			float x = startPointX + (objectWidth + CardSpacing) * (index % Columns);
			int currentRow = index / Columns;
			float y = startPointY - (objectHeight + CardSpacing) * currentRow;//subtract so that the cards go down the y axis
			card.QuadraticOutMoveTo (card.transform.position, new Vector3(x, y, MarketDeck.transform.position.z), animationDuration, () => { card.CurrentCardLocation = CardLocation.CardLocationMarketField; });
			index++;
		}
	}
}
