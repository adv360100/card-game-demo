using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class DiscardActions : BasicAnimator {

	public MeshRenderer[] ExtraCards; //the deck 'stack' cards for when there are more than one card in the deck
	public List<GameObject> CardList = new List<GameObject>();
	public PlayerActions PlayerManager;

	Vector3 AnimationTarget;
	float AnimateRate;
	Func<int, int> CallbackFunction;
	MeshRenderer mainMesh;
	
	void Awake () {
		mainMesh = GetComponent<MeshRenderer> ();
		UpdateDisplay ();
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
		UpdateDisplay ();
	}

	public void RemoveAllCards () {
		CardList.Clear ();
		UpdateDisplay ();
	}

	public void UpdateDisplay () {
		if (CardList.Count > 0)
			mainMesh.enabled = true;
		else
			mainMesh.enabled = false;
		
		for (int i = 0; i < ExtraCards.Length; i++)
		{
			MeshRenderer mesh = ExtraCards[i];
			if (CardList.Count > i + 1)
				mesh.enabled = true;
			else
				mesh.enabled = false;
			
		}
	}

	public override void AnimateTo (Vector3 target, float rate, System.Func<int, int> callback) {
		AnimationTarget = target;
		AnimateRate = rate;
		CallbackFunction = callback;
	}
}
