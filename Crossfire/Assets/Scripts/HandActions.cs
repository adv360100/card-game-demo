using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HandActions : BasicAnimator {

	public List<GameObject> CardList = new List<GameObject>();
	public PlayerArea PlayerManager;

	// Use this for initialization
	void Start () {
	
	}

	public void AddCard (GameObject card) {
		CardList.Add (card);
		card.transform.parent = transform;
		UpdateDeckDisplay ();
	}

	public void AddCards (IEnumerable<GameObject> cardsToAdd) {
		CardList.AddRange (cardsToAdd);
		UpdateDeckDisplay ();
	}

	public void RemoveCard (GameObject card) {
		CardList.Remove (card);
		UpdateDeckDisplay ();
	}

	void UpdateDeckDisplay() {
		UpdateDeckDisplay (1.0f);
	}

	void UpdateDeckDisplay(float animationRate) {
		if (CardList.Count == 0) {
			return;
		}

		float objectWidth = CardList [0].renderer.bounds.size.x;
		int count = CardList.Count - 1;
		float containerWidth = count * objectWidth;
		int index = 0;
		//find start point
		float startPoint = containerWidth * -0.5f;
		foreach (GameObject cardObject in CardList) {
			Card card = cardObject.GetComponent<Card>();
			card.QuadraticOutMoveTo(card.transform.position, new Vector3(startPoint + objectWidth * index, transform.position.y, transform.position.z), animationRate, null);
			index++;
		}
	}
}
