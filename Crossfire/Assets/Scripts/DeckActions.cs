using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//This class handles all the behind the scenes deck management
public class DeckActions : MonoBehaviour {

	public List<GameObject> cardList = new List<GameObject>();
	protected List<GameObject> discardList = new List<GameObject>();

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public int DeckCount()
	{
		return cardList.Count;
	}

	public int DiscardCount()
	{
		return discardList.Count;
	}

	public void AddCards(IEnumerable<GameObject> cardsToAdd)
	{
		cardList.AddRange (cardsToAdd);
	}

	GameObject DrawCard()
	{
		if (cardList.Count <= 0)
		{
			if(discardList.Count <= 0)
			{
				return null;
			}
			//shuffle discard into deck and try draw again
			ShuffleDeck(discardList.ToArray());
			AddCards(discardList);
			discardList.Clear();
		}

		GameObject topCard = cardList [cardList.Count - 1];
		cardList.Remove (topCard);
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
		discardList.AddRange (cardsToDiscard);
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
		GameObject[] deck = cardList.ToArray ();
		ShuffleDeck (deck);
		cardList.Clear ();
		foreach (GameObject c in deck) {
			cardList.Add(c);
		}
	}

}

public class DebugDeckActions : DeckActions {
	
	//get cards in deck for viewing 
	public GameObject[] GetDeck()
	{
		return cardList.ToArray();
	}
}

