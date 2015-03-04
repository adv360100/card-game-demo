using UnityEngine;
using System.Collections;

public abstract class BasicArea : MonoBehaviour {

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

}