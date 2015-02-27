using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BlackMarketArea : BasicArea {

	public List<GameObject> CardList;
	public int Columns = 3;
	public int Rows = 2;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	override public void OnFocusEnter()
	{
		
	}

	override public void OnFocusExit()
	{
		
	}

	public void AnimateTarget(GameObject target, float rate) {
		CardList.Add (target);
		UpdateDeckDisplay (rate);
	}

	void UpdateDeckDisplay(float animationRate)
	{
		float objectWidth = CardList [0].renderer.bounds.size.x;
		float objectHeight = CardList [0].renderer.bounds.size.y;
		int count = Columns - 1;
		float containerWidth = count * objectWidth;
		count = Rows - 1;
		float containerHeight = Rows * objectHeight;
		int index = 0;
		//find start point
		float startPointX = containerWidth * -0.5f;
		float startPointY = containerHeight * -0.5f;
		foreach (GameObject cardObject in CardList) 
		{
			Card card = cardObject.GetComponent<Card>();
			float x = startPointX + objectWidth * (index / Rows);
			float y = startPointY + objectHeight * (index / Columns);
			card.AnimateTo(new Vector3(x, y, transform.position.z),animationRate);
			index++;
		}
	}
}
