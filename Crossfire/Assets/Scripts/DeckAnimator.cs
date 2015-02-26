using UnityEngine;
using System.Collections;

public class DeckAnimator : DeckActions {

	public MeshRenderer[] ExtraCards; //the deck 'stack' cards for when there are more than one card in the deck

	MeshRenderer mainMesh;

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
