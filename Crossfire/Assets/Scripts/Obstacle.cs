using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Obstacle : Card {

	public List<Card> CardList;
	public float CardOverlapSpacing = 2;
	public float TopPlayCardMargin = 0f;

	public void PlayCard(Card card)
	{
		CardList.Add(card);
		card.AttachedObstacle = this;
		card.transform.parent = transform;
		UpdateDeckDisplay (1f);
	}

	public void RemoveCard(Card card)
	{
		CardList.Remove (card);
		card.AttachedObstacle = null;
		UpdateDeckDisplay (1f);
	}

	public void RemoveAllCards()
	{
		CardList.ForEach (delegate(Card obj) {
			obj.AttachedObstacle = null;
		});
		CardList.Clear ();
		UpdateDeckDisplay (1f);
	}

	void UpdateDeckDisplay(float animationDuration)
	{
		if (CardList.Count == 0)
			return;
	
		int index = 0;
		//find start point
		float startPointX = transform.position.x;
		float startPointY = transform.position.y - TopPlayCardMargin;
		foreach (Card card in CardList) 
		{
			card.GetComponent<Renderer>().enabled = true;
			float x = startPointX;
			float y = startPointY - CardOverlapSpacing * index;//subtract so that the cards go down the y axis
			card.QuadraticOutMoveTo (card.transform.position, new Vector3(x, y, transform.position.z - (1 + index)), animationDuration, () => { });
			index++;
		}
	}}
