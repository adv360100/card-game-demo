using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class DiscardDeck : Deck {
	
	Vector3 AnimationTarget;
	float AnimateRate;
	Func<int, int> CallbackFunction;
	MeshRenderer mainMesh;
	
	void Awake () {
		mainMesh = GetComponent<MeshRenderer> ();
		UpdateDeckDisplay ();
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (AnimateRate > 0f) {
			float fraction = Time.deltaTime * (1.0f / AnimateRate);
			transform.position = Vector3.Lerp(transform.position,AnimationTarget,fraction);
			//transform.rotation = Quaternion.Slerp(transform.rotation,AnimateToTransform.rotation,fraction);
			
			if(Vector3.Distance(transform.position, AnimationTarget) <= 0.01f) {
				AnimateRate = 0f;
				transform.position = AnimationTarget;
				CallbackFunction(0);
			}
		}
	}

	public void AddCard (GameObject card) {
		CardList.Add (card);
		UpdateDeckDisplay ();
	}

	public void RemoveAllCards () {
		CardList.Clear ();
		UpdateDeckDisplay ();
	}

	public bool ContainsCard (Card card) {
		return CardList.Contains (card.gameObject);
	}
}
