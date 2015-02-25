using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//This class handles all the behind the scenes deck management
public class DeckActions : MonoBehaviour {

	public List<GameObject> CardList = new List<GameObject>();
	protected List<GameObject> DiscardList = new List<GameObject>();

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public int DeckCount()
	{
		return CardList.Count;
	}

	public int DiscardCount()
	{
		return DiscardList.Count;
	}

	public void AddCards(IEnumerable<GameObject> cardsToAdd)
	{
		CardList.AddRange (cardsToAdd);
	}

	GameObject DrawCard()
	{
		if (CardList.Count <= 0)
		{
			if(DiscardList.Count <= 0)
			{
				return null;
			}
			//shuffle discard into deck and try draw again
			ShuffleDeck(DiscardList.ToArray());
			AddCards(DiscardList);
			DiscardList.Clear();
		}

		GameObject topCard = CardList [CardList.Count - 1];
		CardList.Remove (topCard);
		return topCard;
	}

	public GameObject[] DrawCards(int drawNumber)
	{
		GameObject card;

		GameObject[] drawnCards = new GameObject[drawNumber];
		for (int i=0; i < drawNumber; i++) {
			card = DrawCard();

			if(null == card)
			{
				//no more cards to draw
				break;
			}

			drawnCards[i] = card;
		}

		return drawnCards;
	}

	public void DiscardCards(IEnumerable<GameObject> cardsToDiscard)
	{
		DiscardList.AddRange (cardsToDiscard);
	}
	
	void ShuffleDeck(GameObject[] deck)
	{
		for (int i = 0; i < deck.Length; i++) {
			GameObject temp = deck[i];
			int randomIndex = Random.Range(0, deck.Length);
			deck[i] = deck[randomIndex];
			deck[randomIndex] = temp;
		}
	}

	public void Shuffle()
	{
		GameObject[] deck = CardList.ToArray ();
		ShuffleDeck (deck);
		CardList.Clear ();
		foreach (GameObject c in deck) {
			CardList.Add(c);
		}
	}

}

public class DebugDeckActions : DeckActions {
	
	//get cards in deck for viewing 
	public GameObject[] GetDeck()
	{
		return CardList.ToArray();
	}
}

