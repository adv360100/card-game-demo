using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BasicArea : MonoBehaviour {

	public GameObject BasicCard;
	public delegate void AnimationCompletionCallback();
	public DiscardDeck DiscardPile;
	public Deck MainDeck;

	// Use this for initialization
	virtual public void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	virtual public void OnFocusEnter () {

	}

	virtual public void OnFocusExit () {
		
	}

	virtual public void MoveCard (Card card) {

	}

	public void RemoveDiscardPile (AnimationCompletionCallback completionCallback) {
		Vector3 curPos = DiscardPile.transform.position;
		DiscardPile.QuadraticOutMoveTo (DiscardPile.transform.position, MainDeck.transform.position, 0.25f, () => {
			DiscardPile.RemoveAllCards ();
			DiscardPile.UpdateDeckDisplay ();
			DiscardPile.transform.position = curPos;
			if (completionCallback != null) {
				completionCallback ();
			}
		});
	}

	public List<GameObject> GetDiscardPile () {
		return DiscardPile.CardList;
	}

	public void ShuffleMainDeck () {
		MainDeck.Shuffle ();
	}

	public void SetMainDeck (List<GameObject> cards) {
		SetDeck (cards, MainDeck);
	}

	protected void SetDeck (List<GameObject> cards, Deck deck) {
		if (deck != null) {
			List<GameObject> cardList = new List<GameObject> ();
			for (int i = 0; i < cards.Count; i++) {
				Vector3 newPos = new Vector3 (deck.transform.position.x, deck.transform.position.y, deck.transform.position.z + 1);
				GameObject cardToAdd = GameObject.Instantiate (BasicCard, newPos, deck.transform.rotation) as GameObject;
				//parent card for easier debugging
				cardToAdd.transform.parent = deck.transform;
				cardToAdd.name = cards[i].name;
				cardToAdd.GetComponent<Card> ().ID = cards[i].GetComponent<Card> ().ID;
				cardToAdd.GetComponent<Card> ().FrontTexture = cards[i].GetComponent<Card> ().FrontTexture;
				cardToAdd.GetComponent<Card> ().AreaManager = this;
				cardToAdd.GetComponent<Renderer> ().enabled = false;
				cardToAdd.GetComponent<Renderer> ().material.SetTexture (0, cards[i].GetComponent<Card> ().FrontTexture);
				cardList.Add (cardToAdd);
				Destroy (cards[i]);
			}
			
			deck.AddCards (cardList);
		}
	}

	public uint[] GetMainDeckOrder () {
		return GetDeckOrder (MainDeck);
	}

	public void SetMainDeckOrder (uint[] order) {
		SetDeckOrder (MainDeck, order);
	}

	public void SetDeckOrder (Deck deck, uint[] order) {
		List<GameObject> cardList = new List<GameObject> (order.Length);
		for (int i = 0; i < order.Length; i++) {
			foreach (GameObject item in deck.CardList) {
				Card card = item.GetComponent<Card> ();
				if (card.ID == order[i]) {
					cardList.Add (item);
					break;
				}
			}
		}

		deck.CardList = cardList;
		deck.UpdateDeckDisplay ();
	}

	protected uint[] GetDeckOrder (Deck deck) {
		uint[] order = new uint[deck.DeckCount ()];
		for (int i = 0; i < deck.DeckCount (); i++) {
			order[i] = deck.CardList[i].GetComponent<Card> ().ID;
		}
		return order;
	}
}