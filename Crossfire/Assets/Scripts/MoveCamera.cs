using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MoveCamera : MonoBehaviour {

	public List<BasicArea> TargetAreas;

	private int CurrentAreaIndex = 0;

	// Use this for initialization
	void Start () {
		MoveToArea (CurrentAreaIndex);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void MoveToArea(int targetIndex)
	{
		//check if valid
		if (targetIndex > TargetAreas.Count)
			return;
		BasicArea b;
		if (targetIndex == CurrentAreaIndex)
		{
			b = TargetAreas [CurrentAreaIndex];
			//inform current area
			b.OnFocusExit ();
		}
		//grab the position
		b = TargetAreas [targetIndex];
		transform.position = b.transform.position;
		CurrentAreaIndex = targetIndex;
		//inform area
		b.OnFocusEnter ();
	}
}
