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
		if (MainDeck != null) {
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
			
			MainDeck.AddCards(cardList);
			MainDeck.UpdateDeckDisplay();
		}
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

	public void SetMainDeck (List<GameObject> cards) {
		if (MainDeck != null) {
			List<GameObject> cardList = new List<GameObject> ();
			for (int i = 0; i < cards.Count; i++) {
				Vector3 newPos = new Vector3 (MainDeck.transform.position.x, MainDeck.transform.position.y, MainDeck.transform.position.z + 1);
				GameObject cardToAdd = GameObject.Instantiate (BasicCard, newPos, MainDeck.transform.rotation) as GameObject;
				//parent card for easier debugging
				cardToAdd.transform.parent = MainDeck.transform;
				cardToAdd.name = cards[i].name;
				cardToAdd.GetComponent<Card> ().ID = cards[i].GetComponent<Card> ().ID;
				cardToAdd.GetComponent<Card> ().FrontTexture = cards[i].GetComponent<Card> ().FrontTexture;
				cardToAdd.GetComponent<Card> ().AreaManager = this;
				cardToAdd.GetComponent<Renderer> ().enabled = false;
				cardToAdd.GetComponent<Renderer> ().material.SetTexture (0, cards[i].GetComponent<Card> ().FrontTexture);
				cardList.Add (cardToAdd);
				Destroy (cards[i]);
			}
			
			MainDeck.AddCards (cardList);
			MainDeck.UpdateDeckDisplay ();
		}
	}
}