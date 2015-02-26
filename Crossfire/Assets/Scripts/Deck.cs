using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//This class handles all the behind the scenes deck management
public class Deck : MonoBehaviour {

	public PlayerActions PlayerManager;
	public List<GameObject> CardList = new List<GameObject>();
	public MeshRenderer[] ExtraCards; //the deck 'stack' cards for when there are more than one card in the deck
	MeshRenderer mainMesh;

	public int DeckCount()
	{
		return CardList.Count;
	}

	public void AddCards(IEnumerable<GameObject> cardsToAdd)
	{
		CardList.AddRange (cardsToAdd);
	}

	GameObject DrawCard()
	{
		if (CardList.Count <= 0)
		{
			if(PlayerManager.GetDiscardPile().Count <= 0)
			{
				return null;
			}
			//shuffle discard into deck and try draw again
			AddCards(PlayerManager.GetDiscardPile());
			ShuffleDeck(CardList.ToArray());
			PlayerManager.RemoveDiscardPile();
		}

		GameObject topCard = CardList [CardList.Count - 1];
		topCard.GetComponent<Card>().CurrentCardLocation = CardLocation.CardLocationCurrentPlayer;
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
	
	void Awake(){
		mainMesh = GetComponent<MeshRenderer> ();
		UpdateDeckDisplay ();
	}
	
	// Use this for initialization
	void Start () {
		//deck = GetComponent<DeckActions> ();
		//mainMesh = GetComponent<MeshRenderer> ();
		
		UpdateDeckDisplay ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	void OnMouseDown()
	{
		if (DeckCount () == 0)
			return;
		GameObject[] cards = DrawCards (1);
		foreach (GameObject cardObject in cards) 
		{
			cardObject.transform.position = transform.position;
			PlayerManager.AddCardToHand(cardObject);
		}
		
		UpdateDeckDisplay ();
	}
	
	void UpdateDeckDisplay()
	{
		int deckSize = DeckCount ();
		if (deckSize > 0)
			mainMesh.enabled = true;
		else
			mainMesh.enabled = false;
		
		for (int i=0; i < ExtraCards.Length; i++)
		{
			MeshRenderer mesh = ExtraCards[i];
			if (deckSize > i+1)
				mesh.enabled = true;
			else
				mesh.enabled = false;
			
		}
	}

}

public class DebugDeckActions : Deck {
	
	//get cards in deck for viewing 
	public GameObject[] GetDeck()
	{
		return CardList.ToArray();
	}
}

