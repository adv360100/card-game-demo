using UnityEngine;
using System.Collections;

public class DeckAnimator : MonoBehaviour {

	public Transform Hand;
	public Transform Discard;
	public MeshRenderer[] ExtraCards; //the deck 'stack' cards for when there are more than one card in the deck

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
			c.AnimateCard(transform,Hand,1.0f);
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
