using UnityEngine;
using System.Collections;

public abstract class BasicArea : MonoBehaviour {

	public delegate void AnimationCompletionCallback();
	public DiscardDeck DiscardPile;
	public Deck MainDeck;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	virtual public void OnFocusEnter()
	{

	}
	virtual public void OnFocusExit()
	{
		
	}

	virtual public void MoveCard (Card card) {

	}

	public void RemoveDiscardPile (AnimationCompletionCallback completionCallback) {
		Vector3 curPos = DiscardPile.transform.position;
		DiscardPile.QuadraticOutMoveTo (DiscardPile.transform.position, MainDeck.transform.position, 0.25f, () => {
			DiscardPile.RemoveAllCards ();
			DiscardPile.UpdateDeckDisplay ();
			DiscardPile.transform.position = curPos;
			if (completionCallback != null) {
				completionCallback ();
			}
		});
	}

}