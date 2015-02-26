using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HandActions : BasicAnimator {

	public List<GameObject> CardList = new List<GameObject>();

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void AddCards(IEnumerable<GameObject> cardsToAdd)
	{
		CardList.AddRange (cardsToAdd);
		UpdateDeckDisplay ();
	}

	override public void AnimateTarget(GameObject target, float rate) {
		CardList.Add (target);
		UpdateDeckDisplay (rate);
	}

	void UpdateDeckDisplay()
	{
		UpdateDeckDisplay (1.0f);
	}

	void UpdateDeckDisplay(float animationRate)
	{
		float objectWidth = CardList [0].renderer.bounds.size.x;
		int count = CardList.Count - 1;
		float containerWidth = count * objectWidth;
		int index = 0;
		//find start point
		float startPoint = containerWidth * -0.5f;
		foreach (GameObject cardObject in CardList) 
		{
			Card card = cardObject.GetComponent<Card>();
			card.AnimateTo(new Vector3(startPoint + objectWidth * index, transform.position.y, transform.position.z),animationRate);
			index++;
		}
	}
}
