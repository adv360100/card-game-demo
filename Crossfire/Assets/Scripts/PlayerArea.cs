using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerArea : BasicArea {
	
	public HandActions Hand;
	public GameObject PlayField;
	public GameObject ObstacleSection;
	public GameObject OriginalCard;

	// Use this for initialization
	void Start () {
		// Create the starting deck
		if (MainDeck != null) { 
			List<GameObject> cardList = new List<GameObject>();
			for (int i = 0; i < 5; i++) {
				GameObject cardToAdd = GameObject.Instantiate(OriginalCard, MainDeck.transform.position, MainDeck.transform.rotation) as GameObject;
				cardToAdd.GetComponent<Card>().CurrentCardLocation = CardLocation.CardLocationDeck;
				cardToAdd.GetComponent<Card>().AreaManager = this;
				cardToAdd.GetComponent<Renderer>().enabled = false;
				cardToAdd.transform.parent = MainDeck.transform;

				Vector3 pos = cardToAdd.transform.position;
				pos.z = 1;
				cardToAdd.transform.position = pos;
				cardList.Add(cardToAdd);
			}

			MainDeck.AddCards(cardList);
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public List<GameObject> GetDiscardPile () {
		return DiscardPile.CardList;
	}

	//MoveCardToDiscardFromHand
	override public void MoveCard (Card card) {
		if (DiscardPile.ContainsCard (card)) {
			return;
		}

		card.CurrentCardLocation = CardLocation.CardLocationDiscard;
		Hand.RemoveCard (card.gameObject);
		card.QuadraticOutMoveTo (card.transform.position, DiscardPile.transform.position, 1.0f, () => {
			DiscardPile.AddCard (card.gameObject);
			card.GetComponent<Renderer>().enabled = false;
		});
	}

	public void AddCardToHand (GameObject card) {
		Hand.AddCard (card);
	}

	public void ShowObstacleTargetsUI () {
		
	}

	public void EndTurn () {

	}

	override public void OnFocusEnter()
	{
		
	}
	override public void OnFocusExit()
	{
		
	}
}
