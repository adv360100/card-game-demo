using UnityEngine;
using System.Collections;

public class DeckAnimator : MonoBehaviour {

	public Transform hand;
	public Transform discard;
	public MeshRenderer[] extraCards; //the deck 'stack' cards for when there are more than one card in the deck

	DeckActions deck;
	MeshRenderer mainMesh;

	void Awake(){
		deck = GetComponent<DeckActions> ();
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
		if (deck.DeckCount () == 0)
			return;
		GameObject[] cards = deck.DrawCards (1);
		foreach (GameObject cardObject in cards) 
		{
			Card c = cardObject.GetComponent<Card>();
			c.AnimateCard(transform,hand,1.0f);
		}

		UpdateDeckDisplay ();
	}

	void UpdateDeckDisplay()
	{
		int deckSize = deck.DeckCount ();
		if (deckSize > 0)
			mainMesh.enabled = true;
		else
			mainMesh.enabled = false;

		for (int i=0; i < extraCards.Length; i++)
		{
			MeshRenderer mesh = extraCards[i];
			if (deckSize > i+1)
				mesh.enabled = true;
			else
				mesh.enabled = false;

		}
	}
}
