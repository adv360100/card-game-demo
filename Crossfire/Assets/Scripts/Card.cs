using UnityEngine;
using System.Collections;

public enum CardLocation {CardLocationDeck = 0, CardLocationDiscard, CardLocationCurrentPlayer, CardLocationOtherPlayer};

public class Card : BasicAnimator {

	public int value;
	public CardLocation CurrentCardLocation;
	public PlayerActions PlayerManager;

	Vector3 AnimationTarget;
	float AnimateRate;

	// Use this for initialization
	void Start () {
		AnimateRate = 0f;
	}

	void Awake () {
		CurrentCardLocation = CardLocation.CardLocationDeck;
	}

	void OnMouseDown () {
		if (CurrentCardLocation == CardLocation.CardLocationCurrentPlayer) {
			// Move to discard pile
			PlayerManager.MoveCardToDiscardFromHand(this);
			CurrentCardLocation = CardLocation.CardLocationDiscard;
		}
	}
	
	// Update is called once per frame
//	void Update () {
//		base.Update ();
//		if (AnimateRate > 0f) {
//			float fraction = Time.deltaTime * (1.0f / AnimateRate);
//			transform.position = Vector3.Lerp(transform.position,AnimationTarget,fraction);
//			//transform.rotation = Quaternion.Slerp(transform.rotation,AnimateToTransform.rotation,fraction);
//
//			if(Vector3.Distance(transform.position, AnimationTarget) <= 0.01f) {
//				AnimateRate = 0f;
//				transform.position = AnimationTarget;
//			}
//		}
//	}

	override public void AnimateTo(Vector3 target, float rate) {
		AnimationTarget = target;
		AnimateRate = rate;
	}
}
